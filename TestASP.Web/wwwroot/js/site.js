// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function boolean_choice_click(e,trueClass,falseClass){  
    for(let falseItem of document.getElementsByClassName(falseClass)){
        falseItem.toggle(e.value == "False");
    }
    for(let trueItem of document.getElementsByClassName(trueClass)){
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
const SHOW_PASSWORD_ICON = "fa-regular fa-eye";
const HIDE_PASSWORD_ICON = "fa-regular fa-eye-slash";
function togglePassword(button,inputId)
{
    var input = document.getElementById(inputId);
    var isPassword = input.getAttribute("type") === "password";
    var icon = isPassword ? SHOW_PASSWORD_ICON : HIDE_PASSWORD_ICON;
    button.innerHTML = `<i class="${icon}"/>`;
    input.setAttribute("type", isPassword ? "text" : "password");
}

function toogle(checkbox,viewToHide)
{
    $(`#${viewToHide}`).toggle(checkbox.value);
}