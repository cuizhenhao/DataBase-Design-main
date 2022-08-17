import Vue from 'vue'
import VueRouter from 'vue-router'
import loginRegister from '../views/LoginRegister.vue'
import About from "@/views/About";
import PersonalGamebase from "@/views/PersonalGamebase";
import News from "@/views/News";

Vue.use(VueRouter)

const routes = [
  {
    path:'/login',
    name:'login',
    component: loginRegister
  },
  {
    path:'/GameDetail',
    name:'GameDetail',
    component: About
  },
  {
    path:'/',
    name:'Library',
    component: PersonalGamebase,
  },
  {
    path:'/News',
    name:'News',
    component: News
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
