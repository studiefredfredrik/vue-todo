<template>
  <transition name="modal-fade">
    <div class="modal-backdrop" id="backdrop" v-on:click="backdropClick">
      <div class="modal" role="dialog">
        <div class="wrapper" v-on:click="close">
          <span class="close"></span>
        </div>

        <!-- Blog entry -->
        <div class="w3-card-4 w3-margin w3-white">
          <img v-if="!image.editing && image.image" v-bind:src="image.image" style="width:100%" v-on:click="image.editing = true;">
          <croppa v-if="image.editing"
                  v-model="image.myCroppa"
                  :width="700"
                  :height="220"
          ></croppa>
          <div class="w3-container">
            <input type="text" v-if="heading.editing" v-on:blur="heading.editing = false;" v-model="heading.text"/>
            <h3><b v-if="!heading.editing" v-on:click="heading.editing = true;">{{heading.text}}</b></h3>
            <input type="text" v-if="undertitle.editing" v-on:blur="undertitle.editing = false;" v-model="undertitle.text"/>
            <h5 v-if="!undertitle.editing" v-on:click="undertitle.editing = true;">{{undertitle.text}}</h5>
          </div>

          <div class="w3-container">
            <input type="text" class="widt100" v-if="description.editing" v-on:blur="description.editing = false;" v-model="description.text"/>
            <p id="description" v-if="!description.editing" v-on:click="description.editing = true;">
              {{description.text}}
            </p>

            <input class="widt100" type="text" v-if="more.editing" v-on:blur="more.editing = false;" v-model="more.text"/>
            <p v-if="!more.editing" v-on:click="more.editing = true;">
              {{more.text}}
            </p>
            <br>
          </div>

          <div class="w3-container">
            <div class="w3-row">
              <div class="w3-col m8 s12">
                <p><button class="w3-button w3-padding-large w3-white w3-border" @click="savePost()"><b>Save</b></button></p>
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

  export default {
    name: 'modal',
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
        undertitle: {
          text: `Praesent tincidunt sed`,
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
      backdropClick: function(e){
        if(e.target.id === 'backdrop')
          this.$emit('close')
      },
      savePost: function(){
        console.log(this.image.myCroppa.generateDataUrl());

        let post = {
          image: this.image.myCroppa.generateDataUrl() || this.image.image,
          heading: this.heading.text,
          undertitle: this.undertitle.text,
          description: this.description.text,
          more: this.more.text,
          type: 'equipment'
        }
        console.log('post', post);
        if(this.id){
          axios.put(`/api/Notes?password=PeopleCantPostWithoutPlaying`, post)
            .then(response => {
              // JSON responses are automatically parsed.
              window.location.href = '/';
            })
            .catch(e => {
              this.errors.push(e)
            })
        } else{
          axios.post(`/api/Notes?password=PeopleCantPostWithoutPlaying`, post)
            .then(response => {
              // JSON responses are automatically parsed.
              window.location.href = '/';
            })
            .catch(e => {
              this.errors.push(e)
            })
        }

      },
      close(e) {
        console.log('inner', e.innerHTML)
        this.$emit('close');
      },
    },
    mounted(){
      console.log(this.post)
      if(this.post){
        this.heading.text = this.post.heading
        this.undertitle.text = this.post.undertitle
        this.description.text = this.post.description
        this.more.text = this.post.more
        this.image.image = this.post.image
        if(!this.post.image) this.image.editing = true;
        this.id = this.post.id
      }


    },
    directives: {
      'click-outside': {
        bind: function(el, binding, vNode) {
          // Provided expression must evaluate to a function.
          if (typeof binding.value !== 'function') {
            const compName = vNode.context.name
            let warn = `[Vue-click-outside:] provided expression '${binding.expression}' is not a function, but has to be`
            if (compName) { warn += `Found in component '${compName}'` }

            console.warn(warn)
          }
          // Define Handler and cache it on the element
          const bubble = binding.modifiers.bubble
          const handler = (e) => {
            if (bubble || (!el.contains(e.target) && el !== e.target)) {
              binding.value(e)
            }
          }
          el.__vueClickOutside__ = handler

          // add Event Listeners
          document.addEventListener('click', handler)
        },

        unbind: function(el, binding) {
          // Remove Event Listeners
          document.removeEventListener('click', el.__vueClickOutside__)
          el.__vueClickOutside__ = null

        }
      }
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

  .modal-header,
  .modal-footer {
    padding: 15px;
    display: flex;
  }

  .modal-header {
    border-bottom: 1px solid #eeeeee;
    color: #4AAE9B;
    justify-content: space-between;
  }

  .modal-footer {
    border-top: 1px solid #eeeeee;
    justify-content: flex-end;
  }

  .modal-body {
    position: relative;
    padding: 20px 10px;
  }

  .btn-close {
    border: none;
    font-size: 20px;
    padding: 20px;
    cursor: pointer;
    font-weight: bold;
    color: #4AAE9B;
    background: transparent;
  }

  .btn-green {
    color: white;
    background: #4AAE9B;
    border: 1px solid #4AAE9B;
    border-radius: 2px;
  }
</style>
