
var shortid = require('./js-shortid/dist/js-shortid.min.js');

window.generateUid = function () { return shortid.gen(); }