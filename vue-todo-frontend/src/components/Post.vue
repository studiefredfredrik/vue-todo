<template>
  <div class="modal" role="dialog">
    <div class="wrapper">
      <span class="close"></span>
    </div>

    <div class="w3-card-4 w3-margin w3-white">
      <img v-bind:src="getImageUrl('header.jpg')" style="width:100%">
      <div class="w3-container">
        <h3><b>{{post.heading}}</b></h3>
      </div>

      <div class="w3-container">
        <p id="description">
          <vue-markdown :source="this.post.description"></vue-markdown>
        </p>

        <p>
          <vue-markdown :source="this.post.more"></vue-markdown>
        </p>
        <br>
      </div>
      <div class="w3-container w3-white">
        <p>
          <span class="tags-desc">Tagged with:</span>  <span class="w3-tag 3-light-grey w3-small w3-margin" v-for="(tag, index) in post.tags">{{tag}}</span>
        </p>
      </div>
    </div>



  </div>
</template>

<script>
  import VueMarkdown from 'vue-markdown'
  import axios from 'axios'

  export default {
    name: 'post',
    data: function(){
      return {
        post : {}
      }
    },
    methods: {
      getImageUrl(imageName){
        return `/api/Files/${this.$route.params.id}/${imageName}`
      },
    },
    async created() {
      this.post = (await axios.get(`/api/Notes/${this.$route.params.id}`)).data
      console.log(this.post)
    },
    components: {
      VueMarkdown
    }
  }
</script>

<style scoped>

</style>
