<template>
  <div class="w3-light-grey" ref="container">
    <!-- w3-content defines a container for fixed size centered content,
    and is wrapped around the whole page content, except for the footer in this example -->
    <div class="w3-content" style="max-width:1400px">

      <!-- Header -->
      <header class="w3-container w3-center w3-padding-32">
        <h1><b>MY BLOG</b></h1>
        <p>Welcome to the blog of <span class="w3-tag">unknown</span></p>
      </header>

      <!-- Grid -->
      <div class="w3-row">

        <!-- Blog entries -->
        <div class="w3-col l8 s12">

          <div class="w3-card-4 w3-margin w3-white" v-for="(post, index) in posts">
            <img :src="post.image" alt="Nature" style="width:100%">
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
          <div class="w3-card w3-margin w3-margin-top">
            <img src="/w3images/avatar_g.jpg" style="width:100%">
            <div class="w3-container w3-white">
              <h4><b>My Name</b></h4>
              <p>Just me, myself and I, exploring the universe of uknownment. I have a heart of love and a interest of lorem ipsum and mauris neque quam blog. I want to share my world with you.</p>
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
              <p><span class="w3-tag w3-black w3-margin-bottom">Travel</span> <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">New York</span> <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">London</span>
                <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">IKEA</span> <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">NORWAY</span> <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">DIY</span>
                <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">Ideas</span> <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">Baby</span> <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">Family</span>
                <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">News</span> <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">Clothing</span> <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">Shopping</span>
                <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">Sports</span> <span class="w3-tag w3-light-grey w3-small w3-margin-bottom">Games</span>
              </p>
            </div>
          </div>


            <ul v-if="posts && posts.length">
              <li v-for="post of posts">
                <p><strong>{{post.title}}</strong></p>
                <p>{{post.body}}</p>
              </li>
            </ul>

            <ul v-if="errors && errors.length">
              <li v-for="error of errors">
                {{error.message}}
              </li>
            </ul>

          <!-- END Introduction Menu -->
        </div>

        <!-- END GRID -->
      </div><br>

      <!-- END w3-content -->
    </div>

    <a href="/api/login" class="loginlink">Go to login</a>

  </div>

</template>

<script>
  import editpostmodal from '@/components/EditPostModal.vue'
  import viewpostmodal from '@/components/ViewPostModal.vue'
  import paymentmodal from '@/components/PaymentModal.vue'

  import Vue from 'vue'
  import axios from 'axios';
  import VueMarkdown from 'vue-markdown'

  import toaster from '@/components/ToasterModule'

  export default {
    name: 'Frontpage',
    data () {
      return {
        isModalVisible: false,
        isModalVisible2: false,
        msg: 'Welcome to Your Vue.js App',
        posts: [],
        errors: [],
        loggedIn: false
      }
    },
    methods: {
      showModal(post) {
        let modal = this.loggedIn ? editpostmodal : viewpostmodal
        let ComponentClass = Vue.extend(modal)
        let instance = new ComponentClass({
          propsData: { post: post}
        })
        instance.$mount() // pass nothing
        this.$refs.container.appendChild(instance.$el)
        instance.$on('close', function(refresh){
          instance.$el.remove()
          instance.$destroy()
          if(refresh){
            this.getPosts()
          }
        }.bind(this))
      },
      getPosts: function () {
        axios.get(`/api/Notes`)
          .then(response => {
            // JSON responses are automatically parsed.
            this.posts = response.data
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
      this.getPosts()
    },
    components: {
      VueMarkdown,
      editpostmodal,
      viewpostmodal,
      paymentmodal
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
</style>
