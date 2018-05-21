<template>
  <transition name="modal-fade">
    <div class="modal-backdrop" >
      <div class="modal max50" role="dialog">
        <div class="wrapper" v-on:click="close">
          <span class="close"></span>
        </div>

        <!-- Blog entry -->
        <div class="w3-card-4 w3-margin w3-white">
          <img src="data:image/png;base64,{{post.image}}" alt="Nature" style="width:100%">
          <div class="w3-container">
            <h3><b>{{post.heading}}</b></h3>
            <h5>{{post.undertitle}}</h5>
          </div>

          <div class="w3-container">
            <p>{{post.description}}</p>
            <div class="w3-row">
              <div class="w3-col m8 s12">
                <p><button class="w3-button w3-padding-large w3-white w3-border"><b @click="OpenModal()">READ MORE »</b></button></p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
  export default {
    name: 'readMoreModal',
    props: ['post'],
    data: function() {
      return {
      }
    },
    methods: {
      close() {
        this.$emit('close');
      },
    },
  };
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

  .max50{
    width: 50%;
  }

  .widt100{
    width: 100%;
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

  .modal-header,
  .modal-footer {
    padding: 15px;
    display: flex;
  }

  .modal-header {
    border-bottom: 1px solid #eeeeee;
    color: #4AAE9B;
    justify-content: space-between;
  }

  .modal-footer {
    border-top: 1px solid #eeeeee;
    justify-content: flex-end;
  }

  .modal-body {
    position: relative;
    padding: 20px 10px;
  }

  .btn-close {
    border: none;
    font-size: 20px;
    padding: 20px;
    cursor: pointer;
    font-weight: bold;
    color: #4AAE9B;
    background: transparent;
  }

  .btn-green {
    color: white;
    background: #4AAE9B;
    border: 1px solid #4AAE9B;
    border-radius: 2px;
  }
</style>
