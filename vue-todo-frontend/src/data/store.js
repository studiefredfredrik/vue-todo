import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    posts: [],
    tags: [],
    activeTag: '',
    errors: [],
    frontpage: {},
    sidebar: {},
    loggedIn: false,
    currentPage: 0,
    pageSize: 10,
    notesCount: 0,
    cacheBustId: null,
    top4: []
  }
})
