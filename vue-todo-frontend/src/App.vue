<template>
  <div class="w3-light-grey" ref="container">
    <!-- w3-content defines a container for fixed size centered content,
    and is wrapped around the whole page content, except for the footer in this example -->
    <div class="w3-content" style="max-width:1400px">

      <!-- Header -->
      <header class="w3-container w3-center w3-padding-32" @click="goToFrontpage()">
        <h1><b>{{state.frontpage.heading}}</b></h1>
        <p>{{state.frontpage.undertitle}}</p>
      </header>

      <!-- Add new post button -->
      <p v-if="state.loggedIn" class="create-post-button">
        <button @click="showModal()" class="w3-button w3-white w3-card-4">Create blog post</button>
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
          <div class="w3-card w3-margin w3-margin-top" @click="showFrontpageModal()">
            <img src="/api/Files/frontpage/description-image.jpg"  style="width:100%">
            <div class="w3-container w3-white">
              <p v-if="state.sidebar.description">
                <vue-markdown>{{state.sidebar.description}}</vue-markdown>
              </p>
            </div>
          </div><hr>

          <!-- Posts -->
          <div class="w3-card w3-margin">
            <div class="w3-container w3-padding">
              <h4>Popular Posts</h4>
            </div>
            <ul class="w3-ul w3-hoverable w3-white" v-for="doc in state.top4" @click="showModal(doc)">
              <li class="w3-padding-16">
                <img :src="getImageUrl(doc.id)" alt="Image" class="w3-left w3-margin-right" style="width:50px">
                <span class="w3-large">{{doc.heading}}</span><br>
                <span>{{doc.more.length > 120 ? doc.more.substring(0, 120) + '...' : doc.more}}</span>
                <span class="views-counter">{{doc.views}}</span>
              </li>
            </ul>
          </div>
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

    <!-- Footer -->
    <footer v-if="getNumberOfPages() > 1">
      <a class="left-button" @click="goToPage(state.currentPage-1)" v-if="state.currentPage > 0"><<</a>
      <a class="page-number" @click="goToPage(index)" v-bind:class="{'active-page': state.currentPage+1 === index}" v-for="index in getNumberOfPages()">{{index}}</a>
      <a class="right-button" @click="goToPage(state.currentPage+1)" v-if="state.currentPage < getNumberOfPages()">>></a>
    </footer>

  </div>

</template>

<script>
  import editpostmodal from '@/components/EditPostModal.vue'
  import viewpostmodal from '@/components/ViewPostModal.vue'
  import paymentmodal from '@/components/PaymentModal.vue'
  import editfrontpagemodal from '@/components/EditFrontpageModal.vue'

  import Vue from 'vue'
  import axios from 'axios';
  import VueMarkdown from 'vue-markdown'

  import toaster from '@/components/ToasterModule'
  import Frontpage from "./components/Frontpage";

  import store from './data/store'

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
      getNumberOfPages(){
        return Math.floor(store.state.notesCount / store.state.pageSize) + 1
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
        this.currentPage = pageNumber-1
        this.$router.push({ query: { page: store.state.currentPage }})
        // this.$route.query.page = this.currentPage
        this.getPosts()
      },
      showModal(post) {
        this.$router.push({path: `/post/${post.id}`})
      },
      showFrontpageModal : function(){
        if(!store.state.loggedIn) return
        let ComponentClass = Vue.extend(editfrontpagemodal)
        let instance = new ComponentClass({
          propsData: { frontpage: store.state.frontpage }
        })
        instance.$mount() // pass nothing
        this.$refs.container.appendChild(instance.$el)
        instance.$on('close', function(refresh){
          instance.$el.remove()
          instance.$destroy()
          if(refresh){
            this.getPage()
          }
        }.bind(this))
      },
      goToFrontpage: function(){
        this.$router.push({path: `/`})
      },
      getPosts: function () {
        axios.get(`/api/Notes?pageSize=${store.state.pageSize}&pageNumber=${store.state.currentPage}&tag=${store.state.activeTag}`)
          .then(response => {
            store.state.posts = response.data
          })
          .catch(e => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
      getPage: function () {
        axios.get(`/api/Frontpage`)
          .then(response => {
            store.state.frontpage = response.data
            store.state.sidebar = response.data.sidebar
          })
          .catch(e => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
      getNoteCount: function () {
        axios.get(`/api/Notes/count`)
          .then(response => {
            store.state.notesCount = response.data
          })
          .catch(e => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
      getMostViewedPosts: function () {
        axios.get(`/api/Notes/count`)
          .then(response => {
            store.state.notesCount = response.data
          })
          .catch(e => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
      getTags: function () {
        axios.get(`/api/Tags`)
          .then(response => {
            store.state.tags = response.data
          })
          .catch(e => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
      getTop4: function () {
        axios.get(`/api/Statistics/top4`)
          .then(response => {
            store.state.top4 = response.data
          })
          .catch(e => {
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
      this.activeTag = this.$route.query.tag || ''
      this.currentPage = this.$route.query.page || 0
      this.getPage()
      this.getPosts()
      this.getNoteCount()
      this.getTags()
      this.getMostViewedPosts()
      this.getTop4()
    },
    components: {
      Frontpage,
      VueMarkdown,
      editpostmodal,
      viewpostmodal,
      paymentmodal,
      editfrontpagemodal
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
