<template>
  <transition name="modal-fade">
    <div class="modal-backdrop" id="backdrop" v-on:click="backdropClick">
      <div class="modal" role="dialog">
        <div class="wrapper" v-on:click="close">
          <span class="close"></span>
        </div>

        <div class="w3-card-4 w3-margin w3-white">
          <img v-bind:src="post.image" style="width:100%">
          <div class="w3-container">
            <h3><b>{{post.heading}}</b></h3>
          </div>

          <div class="w3-container">
            <p id="description">
              <vue-markdown>{{post.description}}</vue-markdown>
            </p>

            <p>
              <vue-markdown>{{post.more}}</vue-markdown>
            </p>
            <br>
          </div>

        </div>
      </div>
    </div>
  </transition>
</template>

<script>
  import VueMarkdown from 'vue-markdown'

  export default {
    name: 'viewpostmodal',
    props: ['post'],
    methods: {
      backdropClick: function(e){
        if(e.target.id === 'backdrop')
          this.$emit('close')
      },
      close(e) {
        this.$emit('close', false);
      },
    },
    components: {
      VueMarkdown
    }
  }
</script>

<style>
  .wrapper {
    position: relative;
    display: inline-block;
  }

  .close:before {
    content: '✕';
  }

  .close {
    position: absolute;
    top: 0;
    right: 4px;
    cursor: pointer;
  }

  .modal{
    width: 732px;
  }

  .modal-backdrop {
    position: fixed;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    background-color: rgba(0, 0, 0, 0.3);
    display: flex;
    justify-content: center;
    align-items: center;
  }

  .modal {
    background: #FFFFFF;
    box-shadow: 2px 2px 20px 1px;
    overflow-x: auto;
    display: flex;
    flex-direction: column;
  }

</style>
