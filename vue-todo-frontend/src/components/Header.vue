<template>
  <header class="w3-container w3-center w3-padding-32" @click="goToFrontpage()">
    <h1><b>{{state.frontpage.heading}}</b></h1>
    <p>{{state.frontpage.undertitle}}</p>
  </header>
</template>

<script>
  import store from '../data/store'
  export default {
    name: "Header",
    data: function () {
      return {
        state: store.state
      }
    },
  }
</script>

<style scoped>

</style>
