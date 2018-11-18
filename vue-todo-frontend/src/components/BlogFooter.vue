<template>
  <footer v-if="getNumberOfPages() > 1">
    <a class="left-button" @click="goToPage(state.currentPage-1)" v-if="state.currentPage > 0"><<</a>
    <a class="page-number" @click="goToPage(index)" v-bind:class="{'active-page': state.currentPage+1 === index}" v-for="index in getNumberOfPages()">{{index}}</a>
    <a class="right-button" @click="goToPage(state.currentPage+1)" v-if="state.currentPage < getNumberOfPages()">>></a>
  </footer>
</template>

<script>
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
    }

  }
</script>

<style scoped>

</style>
