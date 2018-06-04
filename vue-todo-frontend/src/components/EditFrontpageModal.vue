<template>
  <transition name="modal-fade">
    <div class="modal-backdrop" id="backdrop" @click="backdropClick">
      <div class="modal" id="modal" role="dialog" @click="modalClick">
        <div class="wrapper" v-on:click="close">
          <span class="close"></span>
        </div>

        <div class="w3-container">
          <input type="text" v-if="page.heading.editing" v-on:blur="page.heading.editing = false;" v-model="page.heading.text"/>
          <h3><b v-if="!page.heading.editing" v-on:click="page.heading.editing = true;">{{page.heading.text}}</b></h3>
        </div>

        <div class="w3-container">
          <input type="text" v-if="page.undertitle.editing" v-on:blur="page.undertitle.editing = false;" v-model="page.undertitle.text"/>
          <h3><b v-if="!page.undertitle.editing" v-on:click="page.undertitle.editing = true;">{{page.undertitle.text}}</b></h3>
        </div>


        <!-- Blog entry -->
        <div class="w3-card-4 w3-margin w3-white">
          <img v-show="!sidebar.image.editing && sidebar.image.image" v-bind:src="sidebar.image.image" style="width:100%" v-on:click="sidebar.image.editing = true;">
          <croppa v-show="sidebar.image.editing"
                  v-model="sidebar.image.myCroppa"
                  :width="700"
                  :height="220"
                  :initial-image="sidebar.image.image"
          ></croppa>

          <div class="w3-container" id="text">
            <textarea type="text" rows="10" class="widt100" v-if="sidebar.description.editing" v-on:blur="sidebar.description.editing = false;" v-model="sidebar.description.text"></textarea>
            <p id="description" v-if="!sidebar.description.editing" v-on:click="sidebar.description.editing = true;">
              <vue-markdown v-if="sidebar.description.text">{{sidebar.description.text}}</vue-markdown>
            </p>
          </div>

          <div class="w3-container">
            <div class="w3-row">
              <div class="w3-col m2 s1">
                <p><button class="w3-button w3-padding-large w3-white w3-border" @click="save()"><b>Save</b></button></p>
              </div>
            </div>
          </div>

        </div>
      </div>
    </div>
  </transition>
</template>

<script>
  import axios from 'axios';
  import VueMarkdown from 'vue-markdown'
  import toaster from '@/components/ToasterModule'

  export default {
    name: 'editfrontpagemodal',
    data: function() {
      return {
        page:{
          heading:{
            text: null,
            editing: false
          },
          undertitle:{
            text: null,
            editing: false
          }
        },
        sidebar:{
          image: {
            myCroppa: {},
            image: null,
            editing: false,
          },
          description:{
            text: null,
            editing: false
          }
        }
      }
    },
    props: ['frontpage'],
    methods: {
      handleNewImage: function(){
        this.sidebar.image.image = this.sidebar.image.myCroppa.generateDataUrl()
        this.sidebar.image.editing = false
      },
      backdropClick: function(e){
        if(e.target.id === 'backdrop')
          this.$emit('close')
      },
      modalClick: function(e) {
        // A simple outside-click handler
        if(e.target.localName === 'div'){
          this.page.heading.editing = false
          this.page.undertitle.editing = false
          this.sidebar.description.editing = false
          this.sidebar.image.editing = false
        }
      },
      save: function(){
        let post = {
          heading : this.page.heading.text,
          undertitle: this.page.undertitle.text,
          sidebar: {
            image: this.sidebar.image.myCroppa.generateDataUrl() || this.sidebar.image.image,
            description: this.sidebar.description.text
          }
        }
        axios.put(`/api/Frontpage`, post)
          .then(response => {
            this.$emit('close', true);
          })
          .catch(e => {
            toaster.show('An error occurred saving the post on the server')
        })
      },
      close(e) {
        this.$emit('close', false);
      },
    },
    mounted(){
      if(this.frontpage){
        this.page.heading.text = this.frontpage.heading
        this.page.undertitle.text = this.frontpage.undertitle
        this.sidebar.image.image = this.frontpage.sidebar.image
        if(!this.sidebar.image.image) this.sidebar.image.editing = true
        this.sidebar.description.text = this.frontpage.sidebar.description
        console.log(this.sidebar.description.text)
      }
    },
    components: {
      VueMarkdown
    }
  }
</script>

<style>
  .wrapper {
    position: relative;
    display: inline-block;
  }

  .close:before {
    content: '✕';
  }

  .close {
    position: absolute;
    top: 0;
    right: 4px;
    cursor: pointer;
  }

  .modal{
    width: 732px;
  }

  .widt100{
    width: 100%;
  }

  .modal-backdrop {
    position: fixed;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    background-color: rgba(0, 0, 0, 0.3);
    display: flex;
    justify-content: center;
    align-items: center;
  }

  .modal {
    background: #FFFFFF;
    box-shadow: 2px 2px 20px 1px;
    overflow-x: auto;
    display: flex;
    flex-direction: column;
  }

</style>
