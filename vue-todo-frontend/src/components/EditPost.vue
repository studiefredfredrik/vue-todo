<template>
  <div v-if="propsLoaded">
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
</template>

<script>
  import axios from 'axios';
  import VueMarkdown from 'vue-markdown'
  import toaster from '@/components/ToasterModule'
  import store from '../data/store'

  export default {
    name: 'EditPost',
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
    methods: {
       savePost: async function(){
        let post = {
          heading: this.heading.text,
          description: this.description.text,
          more: this.more.text,
          id: this.id,
          tags: this.tags.tagsString.split(',').map(item => { return item.trim().trimStart()})
        }
        if(this.id){
          await this.uploadCroppedImage(this.id, 'header', true)
          axios.put(`/api/Notes`, post)
            .then(() => {
              store.state.shouldReloadPosts = true
              this.close()
            })
            .catch(() => {
              toaster.show('An error occurred saving the post on the server')
            })
        } else{
          axios.post(`/api/Notes`, post)
            .then(async response => {
              await this.uploadCroppedImage(response.data.id, 'header', false)
              store.state.shouldReloadPosts = true
              this.close()
            })
            .catch(() => {
              toaster.show('An error occurred saving the post on the server')
            })
        }
      },
      deletePost: function(){
        axios.delete(`/api/Notes?id=${this.id}`)
          .then(() => {
            store.state.shouldReloadPosts = true
            this.close()
          })
          .catch(() => {
            toaster.show('An error occurred deleting the post on the server')
          })
      },
      close() {
        this.$router.push({path: `/`})
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
    async mounted() {
      let post = (await axios.get(`/api/Notes/${this.$route.params.id}`)).data
      this.$nextTick(() => {
        if (post) {
          console.log('tester', this.heading, this.image)
          this.heading.text = post.heading
          this.description.text = post.description
          this.more.text = post.more
          this.image.image = post.image
          if (!post.image) this.image.editing = true;
          this.id = post.id
          this.tags.tags = post.tags || []
          this.tags.tagsString = this.tags.tags.join(',')
        }
        this.propsLoaded = true;
      });

    },
    components: {
      VueMarkdown
    }
  }
</script>

<style>
  .widt100{
    width: 100%;
  }

  .post-container{
    overflow: hidden;
  }

  .tags-desc{
    font-style: italic;
    font-size: 12px;
  }

</style>
