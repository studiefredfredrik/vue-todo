<template>
  <div class="w3-light-grey" ref="container">
    <div class="w3-content" style="max-width:1400px">
      <blog-header></blog-header>

      <create-post-button></create-post-button>

      <div class="w3-row">
        <div class="w3-col l8 s12">
          <router-view></router-view>
        </div>

        <div class="w3-col l4">
          <about></about>
          <hr>
          <sidebar></sidebar>
          <hr>
          <tags></tags>
        </div>

      </div><br>
    </div>
    <blog-footer></blog-footer>
  </div>

</template>

<script>

  import VueMarkdown from 'vue-markdown'
  import store from '@/data/store'
  import About from '@/components/About.vue'
  import BlogHeader from '@/components/BlogHeader'
  import BlogFooter from '@/components/BlogFooter'
  import Tags from '@/components/Tags'
  import Sidebar from '@/components/Sidebar';
  import CreatePostButton from '@/components/CreatePostButton';

  export default {
    name: 'app',
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
    },
    mounted(){
      if(document.cookie.indexOf('user=') > -1){
        store.state.loggedIn = true
      }
    },
    components: {
      Sidebar,
      VueMarkdown,
      BlogHeader,
      BlogFooter,
      About,
      Tags,
      CreatePostButton
    },
  }
</script>

<style scoped>
  body,h1,h2,h3,h4,h5 {
    font-family: "Raleway", sans-serif
  }



</style>
