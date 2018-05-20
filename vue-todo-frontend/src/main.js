import Vue from 'vue'
import App from './App'
import router from './router'
import Croppa from 'vue-croppa';

Vue.config.productionTip = false
Vue.use(Croppa)

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  render: h => h(App),
})
