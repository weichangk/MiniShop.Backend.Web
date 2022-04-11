
function nullableEmailCheck(emailStr) {
    var pattern = /^([\.a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])+/;
    if (emailStr != "" && !pattern.test(emailStr)) {
        return false;
    } else {
        return true;
    }
}

function nullablePhoneCheck(phoneStr) {
    var pattern = /^1\d{10}$/;
    if (phoneStr != "" && !pattern.test(phoneStr)) {
        return false;
    } else {
        return true;
    }
}


function userNameCheck(userNameStr) {
    var pattern = /^[a-zA-Z0-9_-]{4,16}$/;
    if (!pattern.test(userNameStr)) {
        return false;
    } else {
        return true;
    }
}

function passCheck(passStr) {
    var pattern = /^.*(?=.{6,})(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%^&*? ]).*$/;
    if (!pattern.test(passStr)) {
        return false;
    } else {
        return true;
    }
}
