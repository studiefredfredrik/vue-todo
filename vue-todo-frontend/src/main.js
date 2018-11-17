import Vue from 'vue'
import App from './App'
import router from './router'
import Croppa from 'vue-croppa';
import 'vue-croppa/dist/vue-croppa.css';
import VueMarkdown from 'vue-markdown';
import Vuex from 'vuex'
Vue.config.productionTip = false
Vue.use(Croppa)
Vue.use(VueMarkdown)
Vue.use(Vuex)

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  render: h => h(App),
})
