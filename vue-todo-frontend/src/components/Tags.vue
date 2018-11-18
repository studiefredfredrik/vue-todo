<template>
  <div class="w3-card w3-margin" v-if="state.tags.length > 0">
    <div class="w3-container w3-padding">
      <h4>Tags</h4>
    </div>
    <div class="w3-container w3-white">
      <p>
        <span class="w3-tag w3-small w3-margin-bottom w3-margin"
              @click="setActiveTag(tag)"
              v-bind:class="[state.activeTag === tag.tag ? 'w3-black w3-padding' : 'w3-light-grey']"
              v-for="(tag) in state.tags">{{tag.tag}}
        </span>
      </p>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';
  import store from '@/data/store'

  export default {
    name: "Tags",
    data: function () {
      return {
        state: store.state,
        modalOpen: false
      }
    },
    methods:{
      getTags: function () {
        axios.get(`/api/Tags`)
          .then(response => {
            store.state.tags = response.data
          })
          .catch(() => {toaster.show('An error occurred getting tags from the server')})
      },
      getPosts: function () {
        axios.get(`/api/Notes?pageSize=${store.state.pageSize}&pageNumber=${store.state.currentPage}&tag=${store.state.activeTag}`)
          .then(response => {
            store.state.posts = response.data
          })
          .catch(() => {toaster.show('An error occurred getting the posts from the server')})
      },
      setActiveTag(tag){
        store.state.currentPage = 0
        if(store.state.activeTag === tag.tag) store.state.activeTag = ''
        else store.state.activeTag = tag.tag
        this.$router.push({ query: { tag: store.state.activeTag }})
        this.getPosts()
      },
    },
    created(){
      this.getTags()
      store.state.activeTag = this.$route.query.tag || ''
    }
  }
</script>

<style scoped>

</style>
