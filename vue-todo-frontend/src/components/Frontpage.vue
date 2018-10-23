<template>
  <div class="w3-light-grey" ref="container">
    <!-- w3-content defines a container for fixed size centered content,
    and is wrapped around the whole page content, except for the footer in this example -->
    <div class="w3-content" style="max-width:1400px">

      <!-- Header -->
      <header class="w3-container w3-center w3-padding-32" @click="showFrontpageModal()">
        <h1><b>{{frontpage.heading}}</b></h1>
        <p>{{frontpage.undertitle}}</p>
      </header>

      <!-- Add new post button -->
      <p v-if="loggedIn" class="create-post-button">
        <button @click="showModal()" class="w3-button w3-white w3-card-4">Create blog post</button>
      </p>

      <!-- Grid -->
      <div class="w3-row">

        <!-- Blog entries -->
        <div class="w3-col l8 s12">

          <div class="w3-card-4 w3-margin w3-white" v-for="(post, index) in posts">
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

        <!-- Introduction menu -->
        <div class="w3-col l4">
          <!-- About Card -->
          <div class="w3-card w3-margin w3-margin-top" @click="showFrontpageModal()">
            <img src="/api/Files/frontpage/description-image.jpg"  style="width:100%">
            <div class="w3-container w3-white">
              <p v-if="sidebar.description">
                <vue-markdown>{{sidebar.description}}</vue-markdown>
              </p>
            </div>
          </div><hr>

          <!-- Posts -->
          <div class="w3-card w3-margin">
            <div class="w3-container w3-padding">
              <h4>Popular Posts</h4>
            </div>
            <ul class="w3-ul w3-hoverable w3-white">
              <li class="w3-padding-16">
                <img src="/w3images/workshop.jpg" alt="Image" class="w3-left w3-margin-right" style="width:50px">
                <span class="w3-large">Lorem</span><br>
                <span>Sed mattis nunc</span>
              </li>
              <li class="w3-padding-16">
                <img src="/w3images/gondol.jpg" alt="Image" class="w3-left w3-margin-right" style="width:50px">
                <span class="w3-large">Ipsum</span><br>
                <span>Praes tinci sed</span>
              </li>
              <li class="w3-padding-16">
                <img src="/w3images/skies.jpg" alt="Image" class="w3-left w3-margin-right" style="width:50px">
                <span class="w3-large">Dorum</span><br>
                <span>Ultricies congue</span>
              </li>
              <li class="w3-padding-16 w3-hide-medium w3-hide-small">
                <img src="/w3images/rock.jpg" alt="Image" class="w3-left w3-margin-right" style="width:50px">
                <span class="w3-large">Mingsum</span><br>
                <span>Lorem ipsum dipsum</span>
              </li>
            </ul>
          </div>
          <hr>

          <!-- Labels / tags -->
          <div class="w3-card w3-margin">
            <div class="w3-container w3-padding">
              <h4>Tags</h4>
            </div>
            <div class="w3-container w3-white">
              <p>
                <span class="w3-tag w3-black w3-margin-bottom" >Travel</span>
                <span class="w3-tag w3-light-grey w3-small w3-margin-bottom" v-bind:class="{ 'w3-black': activeTag == tag }" v-for="(tag, index) in tags">{{}}</span>
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
      <a class="left-button" @click="goToPage(currentPage-1)" v-if="currentPage > 0"><<</a>
      <a class="page-number" @click="goToPage(index)" v-bind:class="{'active-page': currentPage+1 === index}" v-for="index in getNumberOfPages()">{{index}}</a>
      <a class="right-button" @click="goToPage(currentPage+1)" v-if="currentPage < getNumberOfPages()">>></a>
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

  export default {
    name: 'Frontpage',
    data: function () {
      return {
        posts: [],
        tags: [],
        activeTag: '',
        errors: [],
        frontpage: {},
        sidebar: {},
        loggedIn: false,
        currentPage: 0,
        pageSize: 10,
        notesCount: 0,
        cacheBustId: null
      }
    },
    methods: {
      tagClick(){
        this.currentPage = 0
        if(this.activeTag !== '') this.activeTag = ''
        this.getPosts()
      },
      getNumberOfPages(){
        return Math.floor(this.notesCount / this.pageSize) + 1
      },
      getImageUrl(noteId){
        let cacheBustHash = ''
        if (this.cacheBustId === noteId) {
          this.cacheBustId = null
          cacheBustHash = `?${Date.now()}`
        }
        return `/api/Files/${noteId}/header.jpg${cacheBustHash}`

      },
      goToPage(pageNumber){
        this.currentPage = pageNumber-1
        this.getPosts()
      },
      showModal(post) {
        if(post) axios.post(`/api/Statistics?noteId=${post.id}`)
        let modal = this.loggedIn ? editpostmodal : viewpostmodal
        let ComponentClass = Vue.extend(modal)
        let instance = new ComponentClass({
          propsData: { post: post}
        })
        instance.$mount() // pass nothing
        this.$refs.container.appendChild(instance.$el)
        instance.$on('close', function(refresh, postId){
          instance.$el.remove()
          instance.$destroy()
          if(refresh){
            this.cacheBustId = postId
            this.getPosts(this.currentPage)
            this.getNoteCount()
          }
        }.bind(this))
      },
      showFrontpageModal : function(){
        if(!this.loggedIn) return
        let ComponentClass = Vue.extend(editfrontpagemodal)
        let instance = new ComponentClass({
          propsData: { frontpage: this.frontpage }
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
      getPosts: function () {
        axios.get(`/api/Notes?pageSize=${this.pageSize}&pageNumber=${this.currentPage}&tag=${this.activeTag}`)
          .then(response => {
            // JSON responses are automatically parsed.
            this.posts = response.data
          })
          .catch(e => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
      getPage: function () {
        axios.get(`/api/Frontpage`)
          .then(response => {
            // JSON responses are automatically parsed.
            this.frontpage = response.data
            this.sidebar = response.data.sidebar
          })
          .catch(e => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
      getNoteCount: function () {
        axios.get(`/api/Notes/count`)
          .then(response => {
            // JSON responses are automatically parsed.
            this.notesCount = response.data
          })
          .catch(e => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
    },
    mounted(){
      if(document.cookie.indexOf('user=') > -1){
        this.loggedIn = true
      }
    },
    // Fetches posts when the component is created.
    created() {
      this.getPage()
      this.getPosts()
      this.getNoteCount()
    },
    components: {
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

  footer{
    text-align: center;
  }

</style>
