package com.capstone.sportssocialnetwork.activity;

import android.content.Context;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.ChatAdapter;
import com.capstone.sportssocialnetwork.model.Message;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.database.ChildEventListener;
import com.google.firebase.database.DataSnapshot;
import com.google.firebase.database.DatabaseError;
import com.google.firebase.database.DatabaseReference;
import com.google.firebase.database.FirebaseDatabase;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class InvitationChatActivity extends AppCompatActivity implements ChildEventListener {
    private DatabaseReference mDatabase;
    private int invitationId;
    private DatabaseReference mRef;
    private EditText txtChatMessage;
    private ImageButton btnSubmit;
    private ListView lvMessage;
    private ChatAdapter adapter;
    private String userId;
    private String fullName;
    private String subject;
    private RestService service;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_invitation_chat);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        mDatabase = FirebaseDatabase.getInstance().getReference();
        invitationId = getIntent().getIntExtra("invitationId",-1);
        subject = getIntent().getStringExtra("subject");
        getSupportActionBar().setTitle(subject);
        mRef = mDatabase.child("messages/"+invitationId+"");
        mRef.addChildEventListener(this);
        service = new RestService();
        init();
    }

    private void init() {
        userId = DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
        fullName = DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_FULLNAME,"Anonymous");
        lvMessage = (ListView) findViewById(R.id.lv_message);
        txtChatMessage = (EditText) findViewById(R.id.txt_chat_message);
        btnSubmit = (ImageButton) findViewById(R.id.btn_chat_submit);
        btnSubmit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                writeMessage(userId,fullName,txtChatMessage.getText().toString());
                txtChatMessage.setText("");
            }
        });
        txtChatMessage.clearFocus();
        txtChatMessage.setText("");
        InputMethodManager imm = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);
        imm.hideSoftInputFromWindow(txtChatMessage.getWindowToken(), InputMethodManager.HIDE_NOT_ALWAYS);
        txtChatMessage.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
//                Log.e(TAG, s.toString());
                if (s.length() > 0 ) {
                    btnSubmit.setVisibility(View.VISIBLE);
                } else {
                    btnSubmit.setVisibility(View.GONE);
                }
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });
        adapter  = new ChatAdapter(this,R.layout.item_chat,new ArrayList<Message>());
        lvMessage.setAdapter(adapter);
    }


    private void writeMessage(String userId, String sender, String message) {
        // Create new post at /user-posts/$userid/$postid and at
        // /posts/$postid simultaneously
        String key = mDatabase.child("messages/"+invitationId+"").push().getKey();
        Message mess = new Message(sender,message,userId);
        Map<String, Object> postValues = mess.toMap();

        Map<String, Object> childUpdates = new HashMap<>();
        childUpdates.put("messages/"+invitationId +"/"+ key, postValues);
        mDatabase.updateChildren(childUpdates);

    }

    @Override
    public void onChildAdded(DataSnapshot dataSnapshot, String s) {
        Map<String, String> obj = (Map<String, String>) dataSnapshot.getValue();
        Message message = new Message();
        message.fromMap(obj);
        adapter.add(message);


    }

    @Override
    public void onChildChanged(DataSnapshot dataSnapshot, String s) {

    }

    @Override
    public void onChildRemoved(DataSnapshot dataSnapshot) {

    }

    @Override
    public void onChildMoved(DataSnapshot dataSnapshot, String s) {

    }

    @Override
    public void onCancelled(DatabaseError databaseError) {

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_chat,menu);
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        switch (id){
            case R.id.menu_chat_leave_group:
                service.getAccountService().refuseInvitation(invitationId).enqueue(new Callback<ResponseModel<String>>() {
                    @Override
                    public void onResponse(Call<ResponseModel<String>> call, Response<ResponseModel<String>> response) {
                        if (response.isSuccessful()){
                            if (response.body().isSucceed()){
                                writeMessage("-1","System",fullName+" đã thoát khỏi nhóm tán gẫu");
                                onBackPressed();
                            }else{
                                Toast.makeText(InvitationChatActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                            }
                        }else{
                            Toast.makeText(InvitationChatActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<ResponseModel<String>> call, Throwable t) {
                        Toast.makeText(InvitationChatActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
                    }
                });
              
                return true;
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        mRef.removeEventListener(this);
    }
}
