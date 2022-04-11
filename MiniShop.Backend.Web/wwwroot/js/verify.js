
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

function str_2_32_len(str) {
    var pattern = /^.{2,32}$/;
    if (!pattern.test(str)) {
        return false;
    } else {
        return true;
    }
}

function str_0_64_len(str) {
    var pattern = /^.{0,64}$/;
    if (!pattern.test(str)) {
        return false;
    } else {
        return true;
    }
}
