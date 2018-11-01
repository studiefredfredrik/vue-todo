<template>
  <transition name="modal-fade">
    <div class="modal-backdrop" id="backdrop" @click="backdropClick">
      <div class="modal" id="modal" role="dialog" @click="modalClick">
        <div class="wrapper" v-on:click="close">
          <span class="close"></span>
        </div>

        <!-- Blog entry -->
        <div class="w3-card-4 w3-margin w3-white post-container">
          <croppa v-if="propsLoaded"
                  v-model="image.myCroppa"
                  :width="700"
                  :height="220"
                  :initial-image="getImageUrl('header.jpg')"
          ></croppa>
          <div class="w3-container">
            <input type="text" v-if="heading.editing" v-on:blur="heading.editing = false" v-model="heading.text"/>
            <h3><b v-if="!heading.editing" v-on:click="heading.editing = true">{{heading.text}}</b></h3>
          </div>

          <div class="w3-container" id="text">
            <textarea type="text" rows="10" class="widt100" v-if="description.editing" v-on:blur="description.editing = false" v-model="description.text" tabindex="1"></textarea>
            <p id="description" v-if="!description.editing" v-on:click="description.editing = true">
              <vue-markdown>{{description.text}}</vue-markdown>
            </p>

            <textarea class="widt100" rows="10" type="text" v-if="more.editing" v-on:blur="more.editing = false" v-model="more.text" tabindex="2"></textarea>
            <p v-if="!more.editing" v-on:click="more.editing = true">
              <vue-markdown>{{more.text}}</vue-markdown>
            </p>
            <br>
          </div>

          <!-- tags -->
          <div class="w3-container">
            <input type="text" v-if="tags.editing" v-on:blur="tagsEditingChanged(false)" v-model="tags.tagsString"/>
          </div>
            <div class="w3-container w3-white" v-if="!tags.editing" v-on:click="tagsEditingChanged(true)">
              <p>
                <span class="tags-desc">Tagged with:</span>  <span class="w3-tag 3-light-grey w3-small w3-margin" v-for="(tag, index) in tags.tags">{{tag}}</span>
              </p>
            </div>

          <div class="w3-container">
            <div class="w3-row">
              <div class="w3-col m2 s1">
                <p><button class="w3-button w3-padding-large w3-white w3-border" @click="savePost()" tabindex="3"><b>Save</b></button></p>
              </div>
              <div class="w3-col m4 s6">
                <p><button class="w3-button w3-padding-large w3-white w3-border" @click="deletePost()" tabindex="4"><b>Delete</b></button></p>
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
        id: '',
        propsLoaded : false,
        tags:{
          editing: false,
          tags: [],
          tagsString : ''
        }
      }
    },
    props: ['post'],
    methods: {
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
      savePost: async function(){

        let post = {
          heading: this.heading.text,
          description: this.description.text,
          more: this.more.text,
          id: this.id,
          type: 'equipment',
          tags: this.tags.tagsString.split(',').map(item => { return item.trim().trimStart()})
        }
        if(this.id){
          await this.uploadCroppedImage(this.id, 'header', true)
          axios.put(`/api/Notes`, post)
            .then(response => {
              this.$emit('close', true, post.id);
            })
            .catch(e => {
              toaster.show('An error occurred saving the post on the server')
            })
        } else{
          axios.post(`/api/Notes`, post)
            .then(async response => {
              await this.uploadCroppedImage(response.data.id, 'header', false)
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
        return `/api/Files/${this.id}/${imageName}`
      },
      tagsEditingChanged(nextState){
        this.tags.editing = nextState
        this.tags.tags = this.tags.tagsString.split(',') || []
      },
      async uploadCroppedImage(noteId, imageName, overwrite) {
        await this.image.myCroppa.generateBlob(
          async blob => {
            let formData = new FormData();
            formData.append('files[0]', blob);
            await axios.post(`/api/Files/?folder=${noteId}&fileName=${imageName}.jpg&overwrite=${overwrite ? 'true' : 'false'}`, formData, {headers: {'Content-Type': `multipart/form-data; boundary=${formData.boundary}`}})
          },
          'image/jpeg',
          0.8 // compression
        );
      },
    },
    mounted(){
      if(this.post){
        this.heading.text = this.post.heading
        this.description.text = this.post.description
        this.more.text = this.post.more
        this.image.image = this.post.image
        if(!this.post.image) this.image.editing = true;
        this.id = this.post.id
        this.tags.tags = this.post.tags || []
        this.tags.tagsString = this.tags.tags.join(',')
      }
      this.propsLoaded = true;
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
  
  .post-container{
    overflow: hidden;
  }

  .tags-desc{
    font-style: italic;
    font-size: 12px;
  }

</style>
