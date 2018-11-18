<template>
  <div class="w3-light-grey" ref="container">
    <div class="w3-content" style="max-width:1400px">
      <blog-header></blog-header>

      <!-- Add new post button -->
      <p v-if="state.loggedIn" class="create-post-button">
        <button @click="createPost()" class="w3-button w3-white w3-card-4">Create blog post</button>
      </p>

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
  import axios from 'axios';
  import VueMarkdown from 'vue-markdown'
  import store from './data/store'
  import About from '@/components/About.vue'
  import toaster from '@/components/ToasterModule'
  import BlogHeader from '@/components/BlogHeader'
  import BlogFooter from '@/components/BlogFooter'
  import Tags from '@/components/Tags'
  import Sidebar from "./components/Sidebar";

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
        console.log(store.state)
        if (store.state.loggedIn) this.$router.push({path: `/edit/${post.id}`})
        else this.$router.push({path: `/post/${post.id}`})
      },
      createPost() {
        this.$router.push({path: `/new/post`})
      },
      getPosts: function () {
        axios.get(`/api/Notes?pageSize=${store.state.pageSize}&pageNumber=${store.state.currentPage}&tag=${store.state.activeTag}`)
          .then(response => {
            store.state.posts = response.data
          })
          .catch(this.showError)
      },
      showError: function () {
        toaster.show('An error occurred getting the posts from the server')
      }
    },
    mounted(){
      if(document.cookie.indexOf('user=') > -1){
        store.state.loggedIn = true
      }
    },
    created() {
      this.getPosts()
    },
    components: {
      Sidebar,
      VueMarkdown,
      BlogHeader,
      BlogFooter,
      About,
      Tags
    },
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
  body,h1,h2,h3,h4,h5 {
    font-family: "Raleway", sans-serif
  }

  .loginlink {
    font-size: 10px;
    color: gray;
    font-family: "Raleway", sans-serif;
    margin-left: 10px;
  }

  .create-post-button{
    float: right;
    margin: 18px;
  }

  .left-button{

  }

  .page-number{
    margin: 10px;
    cursor: pointer;
  }

  .right-button{

  }

  .active-page{
    text-decoration: underline;
  }

  .views-counter{
    float: right;
    font-size: 8px;
    font-style: italic;
    padding: 4px;
    border-radius: 5px;
    background: #f1f1f1;
  }

  footer{
    text-align: center;
  }

</style>
