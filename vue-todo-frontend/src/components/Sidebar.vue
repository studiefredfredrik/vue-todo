<template>
  <div class="w3-card w3-margin">
    <div class="w3-container w3-padding">
      <h4>Popular Posts</h4>
    </div>
    <ul class="w3-ul w3-hoverable w3-white" v-for="doc in state.top4" @click="showPost(doc)">
      <li class="w3-padding-16">
        <img :src="getImageUrl(doc.id)" alt="Image" class="w3-left w3-margin-right" style="width:50px">
        <span class="w3-large">{{doc.heading}}</span><br>
        <span>{{doc.more.length > 120 ? doc.more.substring(0, 120) + '...' : doc.more}}</span>
        <span class="views-counter">{{doc.views}}</span>
      </li>
    </ul>
  </div>
</template>

<script>
  import axios from 'axios';
  import store from '@/data/store'
  export default {
    name: "Sidebar",
    data: function () {
      return {
        state: store.state
      }
    },
    methods: {
      showPost(post) {
        if (store.state.loggedIn) this.$router.push({path: `/edit/${post.id}`})
        else this.$router.push({path: `/post/${post.id}`})
      },
      getImageUrl(noteId){
        let cacheBustHash = ''
        if (store.state.cacheBustId === noteId) {
          store.state.cacheBustId = null
          cacheBustHash = `?${Date.now()}`
        }
        return `/api/Files/${noteId}/header.jpg${cacheBustHash}`
      },
      getTop4: function () {
        axios.get(`/api/Statistics/top4`)
          .then(response => {
            store.state.top4 = response.data
          })
          .catch(() => {toaster.show('An error occurred getting top posts from the server')})
      }
    },
    created(){
      this.getTop4()
      store.state.currentPage = this.$route.query.page || 0
    }
  }
</script>

<style scoped>
  .views-counter{
    float: right;
    font-size: 8px;
    font-style: italic;
    padding: 4px;
    border-radius: 5px;
    background: #f1f1f1;
  }
</style>
