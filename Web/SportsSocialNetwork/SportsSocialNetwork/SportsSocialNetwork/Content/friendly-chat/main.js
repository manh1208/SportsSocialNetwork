/**
 * Copyright 2015 Google Inc. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
'use strict';

var _userId = "";
var _invitationId = "";
var _getUserActionName = "";
var _userList = [];
var _userFullName = "";

// Initializes FriendlyChat.
function FriendlyChat(userId, invitationId, getUserActionName) {
    _userId = userId;
    _invitationId = invitationId;
    _getUserActionName = getUserActionName;
    this.checkSetup();
    getUserList();
    for (var i = 0; i < _userList.length; i++) {
        if (_userList[i].Id == _userId) {
            _userFullName = _userList[i].FullName;
        }
    }
    // Shortcuts to DOM Elements.
    this.messageList = document.getElementById('messages');
    this.messageForm = document.getElementById('message-form');
    this.messageInput = document.getElementById('message');
    this.submitButton = document.getElementById('submit');
    //this.submitImageButton = document.getElementById('submitImage');
    this.imageForm = document.getElementById('image-form');
    //this.mediaCapture = document.getElementById('mediaCapture');
    //this.userPic = document.getElementById('user-pic');
    //this.userName = document.getElementById('user-name');
    //this.signInButton = document.getElementById('sign-in');
    this.signOutButton = document.getElementById('sign-out');
    this.leaveGroupChatButton = document.getElementById('btnLeaveGroupChat');
    this.denyGroupChatButton = document.getElementById('btnDenyGroupChat');
    this.acceptGroupChatButton = document.getElementById('btnAcceptGroupChat');
    this.signInSnackbar = document.getElementById('must-signin-snackbar');

    // Saves message on form submit.
    this.messageForm.addEventListener('submit', this.saveMessage.bind(this));
    this.signOutButton.addEventListener('click', this.signOut.bind(this));
    this.leaveGroupChatButton.addEventListener('click', this.LeaveMessage.bind(this));
    this.denyGroupChatButton.addEventListener('click', this.DenyJoinMessage.bind(this));
    this.acceptGroupChatButton.addEventListener('click', this.AcceptJoinMessage.bind(this));
    //this.signInButton.addEventListener('click', this.signIn.bind(this));

    // Toggle for the button.
    var buttonTogglingHandler = this.toggleButton.bind(this);
    this.messageInput.addEventListener('keyup', buttonTogglingHandler);
    this.messageInput.addEventListener('change', buttonTogglingHandler);

    // Events for image upload.
    //this.submitImageButton.addEventListener('click', function () {
    //    this.mediaCapture.click();
    //}.bind(this));
    //this.mediaCapture.addEventListener('change', this.saveImageMessage.bind(this));

    this.initFirebase();
}


FriendlyChat.prototype.AcceptJoinMessage = function(){
    this.messagesRef.push({
        sender: "System",
        message: _userFullName+" đã chấp nhận lời mời tham gia",
        userId: -1
    }).then(function () {
    }.bind(this)).catch(function (error) {
        console.error('Error writing new message to Firebase Database', error);
    });
}

FriendlyChat.prototype.DenyJoinMessage = function() {
    this.messagesRef.push({
        sender: "System",
        message: _userFullName + " đã từ chối lời mời tham gia",
        userId: -1
    }).then(function () {
    }.bind(this)).catch(function (error) {
        console.error('Error writing new message to Firebase Database', error);
    });
}

FriendlyChat.prototype.LeaveMessage = function() {
    this.messagesRef.push({
        sender: "System",
        message: _userFullName + " đã thoát khỏi nhóm tán gẫu",
        userId: -1
    }).then(function () {
    }.bind(this)).catch(function (error) {
        console.error('Error writing new message to Firebase Database', error);
    });
}

function getUserList() {
    $.ajax({
        type: 'POST',
        url: _getUserActionName,
        data: { "invitationId": _invitationId },
        async: false,
        dataType: 'json',
        success: function (data) {
            if (data.Succeed) {
                _userList = data.AdditionalData;
            } else {
                showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
            }
        },
        error: function (data) {
            showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
        },
    });
}

// Sets up shortcuts to Firebase features and initiate firebase auth.
FriendlyChat.prototype.initFirebase = function () {
    // TODO(DEVELOPER): Initialize Firebase.
    this.auth = firebase.auth();
    this.database = firebase.database();
    this.storage = firebase.storage();
    // Initiates Firebase auth and listen to auth state changes.
    //this.auth.onAuthStateChanged(this.onAuthStateChanged.bind(this));
    this.loadMessages();
};

// Loads chat messages history and listens for upcoming ones.
FriendlyChat.prototype.loadMessages = function () {
    // Reference to the /messages/ database path.
    this.messagesRef = this.database.ref('messages/' + _invitationId);
    // Make sure we remove all previous listeners.
    this.messagesRef.off();

    // Loads the last 12 messages and listen for new ones.
    var setMessage = function (data) {
        var val = data.val();
        this.displayMessage(data.key, val.sender, val.userId, val.message, val.imageUrl);
    }.bind(this);
    this.messagesRef.limitToLast(100).on('child_added', setMessage);
    this.messagesRef.limitToLast(100).on('child_changed', setMessage);
};

// Saves a new message on the Firebase DB.
FriendlyChat.prototype.saveMessage = function (e) {
    e.preventDefault();
    // Check that the user entered a message and is signed in.
    if (this.messageInput.value) {
        // Add a new message entry to the Firebase Database.
        this.messagesRef.push({
            sender: _userFullName,
            message: this.messageInput.value,
            userId: _userId
        }).then(function () {
            // Clear message text field and SEND button state.
            FriendlyChat.resetMaterialTextfield(this.messageInput);
            this.toggleButton();
        }.bind(this)).catch(function (error) {
            console.error('Error writing new message to Firebase Database', error);
        });
    }
};

// Sets the URL of the given img element with the URL of the image stored in Firebase Storage.
FriendlyChat.prototype.setImageUrl = function (imageUri, imgElement) {
    imgElement.src = imageUri;

    if (imageUri.startsWith('gs://')) {
        imgElement.src = FriendlyChat.LOADING_IMAGE_URL; // Display a loading image first.
        this.storage.refFromURL(imageUri).getMetadata().then(function (metadata) {
            imgElement.src = metadata.downloadURLs[0];
        });
    } else {
        imgElement.src = imageUri;
    }
};

// Saves a new message containing an image URI in Firebase.
// This first saves the image in Firebase storage.
FriendlyChat.prototype.saveImageMessage = function (event) {
    var file = event.target.files[0];

    // Clear the selection in the file picker input.
    this.imageForm.reset();

    // Check if the file is an image.
    if (!file.type.match('image.*')) {
        var data = {
            message: 'You can only share images',
            timeout: 2000
        };
        this.signInSnackbar.MaterialSnackbar.showSnackbar(data);
        return;
    }

    this.messagesRef.push({
        sender: _userFullName,
        imageUrl: FriendlyChat.LOADING_IMAGE_URL,
        userId: _userId
    }).then(function (data) {

        // Upload the image to Firebase Storage.
        this.storage.ref(_userId + '/' + Date.now() + '/' + file.name)
            .put(file, { contentType: file.type })
            .then(function (snapshot) {
                // Get the file's Storage URI and update the chat message placeholder.
                var filePath = snapshot.metadata.fullPath;
                data.update({ imageUrl: this.storage.ref(filePath).toString() });
            }.bind(this)).catch(function (error) {
                console.error('There was an error uploading a file to Firebase Storage:', error);
            });
    }.bind(this));

};



// Triggers when the auth state change for instance when the user signs-in or signs-out.


// Returns true if user is signed-in. Otherwise false and displays a message.


// Resets the given MaterialTextField.
FriendlyChat.resetMaterialTextfield = function (element) {
    element.value = '';
    //element.parentNode.MaterialTextfield.boundUpdateClassesHandler();
};

// Template for messages.




// A loading image URL.
FriendlyChat.LOADING_IMAGE_URL = 'https://www.google.com/images/spin-32.gif';

// Displays a Message in the UI.
FriendlyChat.prototype.displayMessage = function (key, sender, userId, message, imageUri) {

    FriendlyChat.MESSAGE_TEMPLATE_1 =
    '<div class="chat">' +
    '<div class="chat-avatar" id="avatar' + key + '">' +
    '</div>' +
    '<div class="chat-body">' +
    '<div class="name text-right margin-right-20" style="font-weight:bold"></div>' +
    '<div class="chat-content">' +
    '<div class="message"></div>' +
    '</div>' +
    '</div>' +
    '</div>';

    FriendlyChat.MESSAGE_TEMPLATE_2 =
    '<div class="chat chat-left">' +
    '<div class="chat-avatar" id="avatar' + key + '">' +
    '</div>' +
    '<div class="chat-body">' +
    '<div class="name margin-left-20" style="font-weight:bold"></div>' +
    '<div class="chat-content">' +
    '<div class="message"></div>' +
    '</div>' +
    '</div>' +
    '</div>';

    FriendlyChat.MESSAGE_TEMPLATE_3 =
    '<div class="chat chat-left">' +
    '<div class="chat-body">' +
    '<i class="message margin-left-20"></i>'+
    '</div>' +
    '</div>';

    var div = document.getElementById(key);
    var picUrl = "";
    // If an element for that message does not exists yet we create it.
    if (!div) {
        var container = document.createElement('div');
        if (userId == _userId) {
            container.innerHTML = FriendlyChat.MESSAGE_TEMPLATE_1;
        } else if (userId == -1) {
            container.innerHTML = FriendlyChat.MESSAGE_TEMPLATE_3;
        }
        else
        {
            container.innerHTML = FriendlyChat.MESSAGE_TEMPLATE_2;
        }
        div = container.firstChild;
        div.setAttribute('id', key);
        this.messageList.appendChild(div);
    }
    for (var i = 0; i < _userList.length; i++) {
        if (_userList[i].Id == userId) {
            picUrl = _userList[i].AvatarImage;
        }
    }


    $("#avatar" + key).empty();
    //div.querySelector('.pic').style.backgroundImage = 'url(' + picUrl + ')';
    $("#avatar" + key).append(
'<a style="width:40px" class="avatar" data-toggle="tooltip" href="#" >' +
'<img style="width:40px;height:40px" src="' + picUrl + '" >' +
'</a>')
    if (userId != -1) {
        div.querySelector('.name').textContent = sender;
    }
    var messageElement = div.querySelector('.message');
    if (message) { // If the message is text.
        messageElement.textContent = message;
        // Replace all line breaks by <br>.
        messageElement.innerHTML = messageElement.innerHTML.replace(/\n/g, '<br>');
    } else if (imageUri) { // If the message is an image.
        var image = document.createElement('img');
        image.addEventListener('load', function () {
            this.messageList.scrollTop = this.messageList.scrollHeight;
        }.bind(this));
        this.setImageUrl(imageUri, image);
        messageElement.innerHTML = '';
        messageElement.appendChild(image);
    }
    // Show the card fading-in.
    setTimeout(function () { div.classList.add('visible') }, 1);
    this.messageList.scrollTop = this.messageList.scrollHeight;
    this.messageInput.focus();
};

// Enables or disables the submit button depending on the values of the input
// fields.
FriendlyChat.prototype.toggleButton = function () {
    if (this.messageInput.value) {
        this.submitButton.removeAttribute('disabled');
    } else {
        this.submitButton.setAttribute('disabled', 'true');
    }
};

// Checks that the Firebase SDK has been correctly setup and configured.
FriendlyChat.prototype.checkSetup = function () {
    if (!window.firebase || !(firebase.app instanceof Function) || !window.config) {
        window.alert('You have not configured and imported the Firebase SDK. ' +
            'Make sure you go through the codelab setup instructions.');
    } else if (config.storageBucket === '') {
        window.alert('Your Firebase Storage bucket has not been enabled. Sorry about that. This is ' +
            'actually a Firebase bug that occurs rarely. ' +
            'Please go and re-generate the Firebase initialisation snippet (step 4 of the codelab) ' +
            'and make sure the storageBucket attribute is not empty. ' +
            'You may also need to visit the Storage tab and paste the name of your bucket which is ' +
            'displayed there.');
    }
};

FriendlyChat.prototype.signOut = function () {
    // TODO(DEVELOPER): Sign out of Firebase.
    this.messagesRef = this.database.ref('messages/trash');
};
