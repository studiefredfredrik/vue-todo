export default {
  show: function (message, timeout, fadetime) {
    let id = `toaster${Date.now()}`
    let zindex = parseInt(Date.now().toString().substring(5)) + 1000000
    fadetime = fadetime || 1000
    timeout  = timeout || 3000

    function createToaster(){
      setTimeout(function () {
        let child = document.createElement('div')
        child.innerHTML = `${message}`
        child.setAttribute("style", `
                  position:absolute; 
                  top: 10px; 
                  right:10px; 
                  background-color: rgb(255, 125, 115); 
                  transition: opacity ${fadetime}ms linear;
                  padding: 5px 10px;
                  border-radius: 2px;
                  z-index: ${zindex}
                  `)
        child.setAttribute("id", id)
        document.body.appendChild(child);
      },0)
      setTimeout(function () {
        removeToaster()
      }, timeout)
    }
    function removeToaster(){
      let element = document.getElementById(id);
      setTimeout(function () {
        element.style.opacity = 0
        setTimeout(function () {
          element.parentNode.removeChild(element);
        }, fadetime)
      }, timeout)
    }
    createToaster()
  }
}
