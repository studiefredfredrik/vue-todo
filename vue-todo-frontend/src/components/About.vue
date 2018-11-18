<template>
  <div class="w3-card w3-margin w3-margin-top" @click="showEditAboutModal()">
    <img src="/api/Files/frontpage/description-image.jpg"  style="width:100%">
    <div class="w3-container w3-white">
      <p v-if="state.sidebar.description">
        <vue-markdown>{{state.sidebar.description}}</vue-markdown>
      </p>
    </div>
  </div>
</template>

<script>
  import store from '../data/store'
  import VueMarkdown from 'vue-markdown'
  import EditAbout from '@/components/EditAbout.vue'

  export default {
    name: "About",
    data: function () {
      return {
        state: store.state
      }
    },
    methods: {
      showEditAboutModal : function(){
        if(store.state.shouldReloadPosts){
          this.getFrontPage()
        }
      },
    },
    components: {
      VueMarkdown,
      EditAbout
    }
  }
</script>

<style scoped>

</style>
