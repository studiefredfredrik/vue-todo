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
              <p><button @click="showPost(post)" class="w3-button w3-padding-large w3-white w3-border"><b>READ MORE »</b></button></p>
            </div>
          </div>
        </div>
      </div>

      <!-- END BLOG ENTRIES -->
    </div>
</template>

<script>
  import axios from 'axios';
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
      showPost(post) {
        if (store.state.loggedIn) this.$router.push({path: `/edit/${post.id}`})
        else this.$router.push({path: `/post/${post.id}`})
      },
      getPosts: function () {
        if(!store.state.shouldReloadPosts) return
        store.state.shouldReloadPosts = false
        axios.get(`/api/Notes?pageSize=${store.state.pageSize}&pageNumber=${store.state.currentPage}&tag=${store.state.activeTag}`)
          .then(response => {
            store.state.posts = response.data
          })
          .catch(this.showError)
      },
    },
    components: {
      VueMarkdown
    },
    mounted(){
      this.getPosts()
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
