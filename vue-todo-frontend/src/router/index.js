import Vue from 'vue'
import Router from 'vue-router'
import Frontpage from '@/components/Frontpage'
import Post from '@/components/Post'
import EditPost from '@/components/EditPost'


Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Frontpage',
      component: Frontpage
    },
    {
      path: '/post/:id',
      name: 'View post',
      component: Post
    },
    {
      path: '/edit/:id',
      name: 'Edit post',
      component: EditPost
    },
    {
      path: '/new/post',
      name: 'New post',
      component: EditPost
    },
  ]
})
