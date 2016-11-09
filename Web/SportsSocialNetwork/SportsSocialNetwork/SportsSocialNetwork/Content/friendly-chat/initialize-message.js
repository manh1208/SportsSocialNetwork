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
var _userFullName = "";
var _mess = "";

// Initializes FriendlyChat.
function FriendlyChat(userId, invitationId, userFullName, mess) {
    _userId = userId;
    _invitationId = invitationId;
    _userFullName = userFullName;
    _mess = mess;
    this.checkSetup();
    // Shortcuts to DOM Elements.
    this.submit = document.getElementById('submitForm');
    // Saves message on form submit.
    this.submit.addEventListener('click', this.saveMessage.bind(this));
    this.initFirebase();
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


// Saves a new message on the Firebase DB.
FriendlyChat.prototype.saveMessage = function () {
        this.messagesRef.push({
            sender: _userFullName,
            message: _mess,
            userId: _userId
        }).then(function () {
        }.bind(this)).catch(function (error) {
            console.error('Error writing new message to Firebase Database', error);
        });
    
};

FriendlyChat.prototype.loadMessages = function () {
    // Reference to the /messages/ database path.
    this.messagesRef = this.database.ref('messages/' + _invitationId);
    // Make sure we remove all previous listeners.
    this.messagesRef.off();

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


