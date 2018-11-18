<template>
  <p v-if="state.loggedIn" class="create-post-button">
    <button @click="createPost()" class="w3-button w3-white w3-card-4">Create blog post</button>
  </p>
</template>

<script>
  import store from '@/data/store'
  export default {
    name: "CreatePostButton",
    data: function () {
      return {
        state: store.state
      }
    },
    methods:{
      createPost() {
        this.$router.push({path: `/new/post`})
      }
    }
  }
</script>

<style scoped>
  .create-post-button{
    float: right;
    margin: 18px;
  }
</style>
