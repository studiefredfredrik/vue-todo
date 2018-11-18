<template>
  <header class="w3-container w3-center w3-padding-32" @click="goToFrontpage()">
    <h1><b>{{state.frontpage.heading}}</b></h1>
    <p>{{state.frontpage.undertitle}}</p>
  </header>
</template>

<script>
  import store from '../data/store'
  export default {
    name: "BlogHeader",
    data: function () {
      return {
        state: store.state
      }
    },
    methods: {
      goToFrontpage: function(){
        this.$router.push({path: `/`})
      }
    }
  }
</script>

<style scoped>

</style>
