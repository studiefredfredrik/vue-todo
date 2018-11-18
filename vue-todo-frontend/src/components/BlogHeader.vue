<template>
  <header class="w3-container w3-center w3-padding-32" @click="goToFrontpage()">
    <h1><b>{{state.frontpage.heading}}</b></h1>
    <p>{{state.frontpage.undertitle}}</p>
  </header>
</template>

<script>
  import store from '@/data/store'
  export default {
    name: "BlogHeader",
    data: function () {
      return {
        state: store.state
      }
    },
    methods: {
      goToFrontpage: function(){
        store.state.activeTag = ''
        store.state.currentPage = 0
        store.state.shouldReloadPosts = true
        this.$router.push({page: '/', query: { page: store.state.currentPage}})
      }
    }
  }
</script>

<style scoped>

</style>
