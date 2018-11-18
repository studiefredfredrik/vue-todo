<template>
  <div class="w3-card w3-margin w3-margin-top" @click="showEditAboutModal()" ref="about">
    <img src="/api/Files/frontpage/description-image.jpg"  style="width:100%">
    <div class="w3-container w3-white">
      <p v-if="state.sidebar.description">
        <vue-markdown>{{state.sidebar.description}}</vue-markdown>
      </p>
    </div>
  </div>
</template>

<script>
  import Vue from 'vue'
  import axios from 'axios';
  import store from '../data/store'
  import VueMarkdown from 'vue-markdown'
  import EditAbout from '@/components/EditAbout.vue'

  export default {
    name: "About",
    data: function () {
      return {
        state: store.state,
        modalOpen: false
      }
    },
    methods: {
      showEditAboutModal : function(){
        if(!store.state.loggedIn || this.modalOpen) return
        this.modalOpen = true
        let ComponentClass = Vue.extend(EditAbout)
        let instance = new ComponentClass({
          propsData: { frontpage: store.state.frontpage }
        })
        instance.$mount()
        this.$refs.about.appendChild(instance.$el)
        instance.$on('close', function(refresh){
          instance.$el.remove()
          instance.$destroy()
          if(refresh){
            this.getAbout()
          }
          setTimeout(()=>this.modalOpen = false, 100) // TODO: remove this hack xD
        }.bind(this))
      },
      getAbout: function () {
        axios.get(`/api/Frontpage`)
          .then(response => {
            store.state.frontpage = response.data
            store.state.sidebar = response.data.sidebar
          })
          .catch(() => {
            toaster.show('An error occurred getting the posts from the server')
          })
      },
    },
    components: {
      VueMarkdown,
      EditAbout
    },
    created(){
      this.modalOpen = false
      this.getAbout()
    }
  }
</script>

<style scoped>

</style>
