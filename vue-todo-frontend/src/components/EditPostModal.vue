<template>
  <transition name="modal-fade">
    <div class="modal-backdrop" id="backdrop" @click="backdropClick">
      <div class="modal" id="modal" role="dialog" @click="modalClick">
        <div class="wrapper" v-on:click="close">
          <span class="close"></span>
        </div>

        <!-- Blog entry -->
        <div class="w3-card-4 w3-margin w3-white">
          <!--<img v-show="!image.editing && image.image" v-bind:src="getImageUrl('header')" style="width:100%" v-on:click="image.editing = true;">-->
          <croppa v-model="image.myCroppa"
                  :width="700"
                  :height="220"
                  :initial-image="getImageUrl('header')"
          ></croppa>
          <div class="w3-container">
            <input type="text" v-if="heading.editing" v-on:blur="heading.editing = false;" v-model="heading.text"/>
            <h3><b v-if="!heading.editing" v-on:click="heading.editing = true;">{{heading.text}}</b></h3>
          </div>

          <div class="w3-container" id="text">
            <textarea type="text" rows="10" class="widt100" v-if="description.editing" v-on:blur="description.editing = false;" v-model="description.text"></textarea>
            <p id="description" v-if="!description.editing" v-on:click="description.editing = true;">
              <vue-markdown>{{description.text}}</vue-markdown>
            </p>

            <textarea class="widt100" rows="10" type="text" v-if="more.editing" v-on:blur="more.editing = false;" v-model="more.text"></textarea>
            <p v-if="!more.editing" v-on:click="more.editing = true;">
              <vue-markdown>{{more.text}}</vue-markdown>
            </p>
            <br>
          </div>

          <div class="w3-container">
            <div class="w3-row">
              <div class="w3-col m2 s1">
                <p><button class="w3-button w3-padding-large w3-white w3-border" @click="savePost()"><b>Save</b></button></p>
              </div>
              <div class="w3-col m4 s6">
                <p><button class="w3-button w3-padding-large w3-white w3-border" @click="deletePost()"><b>Delete</b></button></p>
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
    name: 'editpostmodal',
    data: function() {
      return {
        image: {
          myCroppa: {},
          image: null,
          editing: false,
        },
        heading: {
          text: `BLOG ENTRY`,
          editing: false,
        },
        description: {
          text: `Mauris neque quam, fermentum ut nisl vitae, convallis maximus nisl.
                  Sed mattis nunc id lorem euismod placerat.
                  Vivamus porttitor magna enim, ac accumsan tortor cursus at. Phasellus sed ultricies mi non congue ullam corper.
                  Praesent tincidunt sed
                  tellus ut rutrum. Sed vitae justo condimentum, porta lectus vitae, ultricies congue gravida diam non fringilla.`,
          editing: false,
        },
        more: {
          text: `Even more info here, with more details etc. And more more more`,
          editing: false,
        },
        id: ''
      }
    },
    props: ['post'],
    methods: {
      handleNewImage: function(){
        // this.uploadCroppedImage('header')
        this.image.editing = false
      },
      backdropClick: function(e){
        if(e.target.id === 'backdrop')
          this.$emit('close')
      },
      modalClick: function(e) {
        // A simple outside-click handler
        if(e.target.localName === 'div'){
          this.heading.editing = false
          this.description.editing = false
          this.more.editing = false
        }
      },
      savePost: function(){

        let post = {
          heading: this.heading.text,
          description: this.description.text,
          more: this.more.text,
          id: this.id,
          type: 'equipment'
        }
        if(this.id){
          this.uploadCroppedImage(this.id, 'header')
          axios.put(`/api/Notes`, post)
            .then(response => {
              this.$emit('close', true);
            })
            .catch(e => {
              toaster.show('An error occurred saving the post on the server')
            })
        } else{
          axios.post(`/api/Notes`, post)
            .then(response => {
              this.uploadCroppedImage(response.data.id, 'header')
              this.$emit('close', true);
            })
            .catch(e => {
              toaster.show('An error occurred saving the post on the server')
            })
        }
      },
      deletePost: function(){
        axios.delete(`/api/Notes?id=${this.id}`)
          .then(response => {
            this.$emit('close', true);
          })
          .catch(e => {
            toaster.show('An error occurred deleting the post on the server')
          })
      },
      close(e) {
        this.$emit('close', false);
      },
      getImageUrl(imageName){
        return `/api/Image?noteId=${this.id}&imageName=${imageName}`
      },
      uploadCroppedImage(noteId, imageName) {
        this.image.myCroppa.generateBlob(
          blob => {
            let formData = new FormData();
            formData.append('files[0]', blob);
            axios.post(`/api/Image/?noteId=${noteId}&imageName=${imageName}`, formData, {headers: {'Content-Type': `multipart/form-data; boundary=${formData.boundary}`}})
              .then(response => {
                toaster.show(`Image saved as ${response.data.guid}`)
              })
              .catch(e => {
                toaster.show('An error occurred saving the post on the server')
              })
          },
          'image/jpeg',
          0.8
        ); // 80% compressed jpeg file
      }
    },
    mounted(){
      if(this.post){
        this.heading.text = this.post.heading
        this.description.text = this.post.description
        this.more.text = this.post.more
        this.image.image = this.post.image
        if(!this.post.image) this.image.editing = true;
        this.id = this.post.id
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
