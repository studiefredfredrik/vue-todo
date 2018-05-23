<template>
  <transition name="modal-fade">
    <div class="modal-backdrop" @click="close">
      <div class="modal max50" role="dialog">
        <div class="wrapper" v-on:click="close">
          <span class="close"></span>
        </div>

        <!-- Blog entry -->
        <div class="w3-card-4 w3-margin w3-white ">
          <img src="/w3images/bridge.jpg" alt="Norway" style="width:100%">
          <div class="w3-container">
            <input type="text" v-if="heading.editing" v-on:blur="heading.editing = false;" v-model="heading.text"/>
            <h3><b v-if="!heading.editing" v-on:click="heading.editing = true;">{{heading.text}}</b></h3>
            <input type="text" v-if="undertitle.editing" v-on:blur="undertitle.editing = false;" v-model="undertitle.text"/>
            <h5 v-if="!undertitle.editing" v-on:click="undertitle.editing = true;">{{undertitle.text}}</h5>
          </div>

          <div class="w3-container">
            <input type="text" class="widt100" v-if="description.editing" v-on:blur="description.editing = false;" v-model="description.text"/>
            <p id="description" v-if="!description.editing" v-on:click="description.editing = true;">
              {{description.text}}
            </p>

            <input class="widt100" type="text" v-if="more.editing" v-on:blur="more.editing = false;" v-model="more.text"/>
            <p v-if="!more.editing" v-on:click="more.editing = true;">
              {{more.text}}
            </p>
            <br>
          </div>

          <div class="w3-container">
            <div class="w3-row">
              <div class="w3-col m8 s12">
                <p><button class="w3-button w3-padding-large w3-white w3-border"><b>Save</b></button></p>
              </div>
            </div>
          </div>

          <croppa v-model="myCroppa"></croppa>

        </div>
      </div>
    </div>
  </transition>
</template>

<script>
  export default {
    name: 'imageModal',
    data: function() {
      return {
        myCroppa: {},
        heading: {
          text: `BLOG ENTRY`,
          editing: false,
        },
        undertitle: {
          text: `Praesent tincidunt sed`,
          editing: false,
        },
        description: {
          text: `Mauris neque quam, fermentum ut nisl vitae, convallis maximus nisl.
              Sed mattis nunc id lorem euismod placerat.
              Vivamus porttitor magna enim, ac accumsan tortor cursus at. Phasellus sed ultricies mi non congue ullam corper.
              Praesent tincidunt sed
              tellus ut rutrum. Sed vitae justo condimentum, porta lectus vitae, ultricies congue gravida diam non fringilla.`,
          editing: false,
        },
        more: {
          text: `Even more info here, with more details etc. And more more more`,
          editing: false,
        },
      }
    },
    methods: {
      close(e) {
        console.log('inner', e.innerHTML)
        this.$emit('close');
      },
      uploadCroppedImage() {
        this.myCroppa.generateBlob(
          blob => {
            // write code to upload the cropped image file (a file is a blob)
          },
          'image/jpeg',
          0.8
        ); // 80% compressed jpeg file
      }
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
