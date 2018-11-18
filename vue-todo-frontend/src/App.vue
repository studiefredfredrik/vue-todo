<template>
  <div class="w3-light-grey" ref="container">
    <!-- w3-content defines a container for fixed size centered content,
    and is wrapped around the whole page content, except for the footer in this example -->
    <div class="w3-content" style="max-width:1400px">

      <!-- Header -->
      <blog-header></blog-header>

      <!-- Add new post button -->
      <p v-if="state.loggedIn" class="create-post-button">
        <button @click="createPost()" class="w3-button w3-white w3-card-4">Create blog post</button>
      </p>

      <!-- Grid -->
      <div class="w3-row">

        <!-- Blog entries -->
        <div class="w3-col l8 s12">

          <router-view></router-view>

          <!-- END BLOG ENTRIES -->
        </div>

        <!-- Introduction menu -->
        <div class="w3-col l4">
          <!-- About Card -->
          <about></about>
          <hr>

          <!-- Posts -->
          <sidebar></sidebar>
          <hr>

          <!-- Labels / tags -->
          <div class="w3-card w3-margin" v-if="state.tags.length > 0">
            <div class="w3-container w3-padding">
              <h4>Tags</h4>
            </div>
            <div class="w3-container w3-white">
              <p>
                <span class="w3-tag w3-small w3-margin-bottom w3-margin"
                      @click="setActiveTag(tag)"
                      v-bind:class="[state.activeTag === tag.tag ? 'w3-black w3-padding' : 'w3-light-grey']"
                      v-for="(tag, index) in state.tags">{{tag.tag}}
                </span>
              </p>
            </div>
          </div>

          <!-- END Introduction Menu -->
        </div>

        <!-- END GRID -->
      </div><br>

      <!-- END w3-content -->
    </div>

    <!--<a href="/api/login" class="loginlink">Go to login</a>-->

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
  import Sidebar from "./components/Sidebar";

  export default {
    name: 'app',
    data: function () {
      return {
        state: store.state
      }
    },
    methods: {
      setActiveTag(tag){
         store.state.currentPage = 0
        if(store.state.activeTag === tag.tag) store.state.activeTag = ''
        else store.state.activeTag = tag.tag
        //this.$route.query.tag = this.activeTag
        this.$router.push({ query: { tag: store.state.activeTag }})
        this.getPosts()
      },
      getImageUrl(noteId){
        let cacheBustHash = ''
        if (store.state.cacheBustId === noteId) {
          store.state.cacheBustId = null
          cacheBustHash = `?${Date.now()}`
        }
        return `/api/Files/${noteId}/header.jpg${cacheBustHash}`

      },
      goToPage(pageNumber){
        store.state.currentPage = pageNumber-1
        this.$router.push({ query: { page: store.state.currentPage }})
        // this.$route.query.page = this.currentPage
        this.getPosts()
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
          .catch(() => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
      getFrontPage: function () {
        axios.get(`/api/Frontpage`)
          .then(response => {
            store.state.frontpage = response.data
            store.state.sidebar = response.data.sidebar
          })
          .catch(() => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
      getNoteCount: function () {
        axios.get(`/api/Notes/count`)
          .then(response => {
            store.state.notesCount = response.data
          })
          .catch(() => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
      getMostViewedPosts: function () {
        axios.get(`/api/Notes/count`)
          .then(response => {
            store.state.notesCount = response.data
          })
          .catch(() => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
      getTags: function () {
        axios.get(`/api/Tags`)
          .then(response => {
            store.state.tags = response.data
          })
          .catch(() => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
      getTop4: function () {
        axios.get(`/api/Statistics/top4`)
          .then(response => {
            store.state.top4 = response.data
          })
          .catch(() => {
            toaster.show('An error occurred getting the posts from the server')
          })
      }
    },
    mounted(){
      if(document.cookie.indexOf('user=') > -1){
        store.state.loggedIn = true
      }
    },
    // Fetches posts when the component is created.
    created() {
      store.state.activeTag = this.$route.query.tag || ''
      store.state.currentPage = this.$route.query.page || 0
      this.getFrontPage()
      this.getPosts()
      this.getNoteCount()
      this.getTags()
      this.getMostViewedPosts()
      this.getTop4()
    },
    components: {
      Sidebar,
      VueMarkdown,
      EditAbout,
      BlogHeader,
      BlogFooter,
      About
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
