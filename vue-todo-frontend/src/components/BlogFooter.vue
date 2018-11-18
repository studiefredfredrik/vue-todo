<template>
  <footer v-if="getNumberOfPages() > 1">
    <a class="left-button" @click="goToPage(state.currentPage-1)" v-if="state.currentPage > 0"><<</a>
    <a class="page-number" @click="goToPage(index)" v-bind:class="{'active-page': state.currentPage+1 === index}" v-for="index in getNumberOfPages()">{{index}}</a>
    <a class="right-button" @click="goToPage(state.currentPage+1)" v-if="state.currentPage < getNumberOfPages()">>></a>
  </footer>
</template>

<script>
  import axios from 'axios';
  import store from '../data/store'
  export default {
    name: "BlogFooter",
    data: function () {
      return {
        state: store.state
      }
    },
    methods: {
      getNumberOfPages(){
        return Math.floor(store.state.notesCount / store.state.pageSize) + 1
      },
      getPostCount: function () {
        axios.get(`/api/Notes/count`)
          .then(response => {
            store.state.notesCount = response.data
          })
          .catch(this.showError)
      },
      goToPage(pageNumber){
        store.state.currentPage = pageNumber-1
        this.$router.push({ query: { page: store.state.currentPage }})
        this.getPosts()
      },
    },
    created(){
      this.getPostCount()
    }
  }
</script>

<style scoped>

</style>
