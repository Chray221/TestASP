// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function boolean_choice_click(e,trueClass,falseClass){  
    console.log(`e:${e} trueClass:${trueClass} falseClass:${falseClass}`);
    for(let falseItem of document.getElementsByClassName(falseClass)){
        // if(e.value == "False"){
        //     falseItem.removeAttribute("hidden");
        // }
        // else {
        //     falseItem.setAttribute("hidden","");
        // }        
        falseItem.toggle(e.value == "False");
    }
    for(let trueItem of document.getElementsByClassName(trueClass)){
        // if(e.value == "True"){
        //     // trueItem.removeAttribute("hidden");
        // }
        // else {
        //     // trueItem.setAttribute("hidden","");
        //     // trueItem.show()
        // }
        trueItem.toggle(e.value == "True");
    }
}

Element.prototype.toggle  = function toggle( isShow)
{
    console.log(`${this.className}: isShowing? ${isShow}`);
    isShow = isShow ?? !this.hasAttribute("hidden");
    if(!isShow){
        this.hide();
    }
    else {
        this.show();
    }
    
}

Element.prototype.hide  = function hide()
{
    hideElement(this);
}

Element.prototype.show  = function show()
{
    showElement(this);
}

/**
 * @param {Element} e The date
 */
function hideElement(e)
{
    e.setAttribute("hidden","");
}

/**
 * @param {Element} e The date
 */
function showElement(e)
{
    e.removeAttribute("hidden");
}