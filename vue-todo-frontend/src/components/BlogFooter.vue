<template>
  <footer v-if="getNumberOfPages() > 1">
    <a class="left-button" @click="goToPage(state.currentPage-1)" v-if="state.currentPage > 0"><<</a>
    <a class="page-number" @click="goToPage(index-1)" v-bind:class="{'active-page': state.currentPage === index-1}" v-for="index in getNumberOfPages()">{{index}}</a>
    <a class="right-button" @click="goToPage(state.currentPage+1)" v-if="state.currentPage < getNumberOfPages() -1">>></a>
  </footer>
</template>

<script>
  import axios from 'axios';
  import store from '@/data/store'
  import toaster from '@/components/ToasterModule'
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
          .catch(() => {toaster.show('An error occurred getting posts from the server')})
      },
      goToPage(pageNumber){
        store.state.currentPage = pageNumber
        store.state.shouldReloadPosts = true
        this.$router.push({ query: { page: store.state.currentPage }})
      }
    },
    created(){
      this.getPostCount()
    }
  }
</script>

<style scoped>
  footer{
    text-align: center;
  }
  .active-page{
    text-decoration: underline;
  }
  .page-number{
    margin: 10px;
    cursor: pointer;
  }
</style>
