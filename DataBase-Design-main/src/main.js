import Vue from 'vue'
import axios from 'axios'
import App from './App.vue'
import router from './router'
import VueResource from 'vue-resource'
import '../public/reset.css'

import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
Vue.use(ElementUI)

Vue.prototype.$axios = axios
Vue.config.productionTip = false

Vue.use(VueResource)

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
