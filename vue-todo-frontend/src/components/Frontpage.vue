<template>
    <!-- Blog entries -->
    <div>

      <div class="w3-card-4 w3-margin w3-white" v-for="(post, index) in state.posts">
        <img :src="getImageUrl(post.id)" style="width:100%">
        <div class="w3-container">
          <h3><b>{{post.heading}}</b></h3>
        </div>

        <div class="w3-container">
          <p>
            <vue-markdown>{{post.description}}</vue-markdown>
          </p>
          <div class="w3-row">
            <div class="w3-col m8 s12">
              <p><button @click="showModal(post)" class="w3-button w3-padding-large w3-white w3-border"><b>READ MORE »</b></button></p>
            </div>
          </div>
        </div>
      </div>

      <!-- END BLOG ENTRIES -->
    </div>
</template>

<script>
  import VueMarkdown from 'vue-markdown'
  import store from '../data/store'
  export default {
    name: 'Frontpage',
    data: function () {
      return {
        state: store.state
      }
    },
    methods: {
      getImageUrl(noteId){
        let cacheBustHash = ''
        if (store.state.cacheBustId === noteId) {
          store.state.cacheBustId = null
          cacheBustHash = `?${Date.now()}`
        }
        return `/api/Files/${noteId}/header.jpg${cacheBustHash}`
      },
      showModal(post) {
        this.$router.push({path: `/post/${post.id}`})
      },
    },
    components: {
      VueMarkdown
    },
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
