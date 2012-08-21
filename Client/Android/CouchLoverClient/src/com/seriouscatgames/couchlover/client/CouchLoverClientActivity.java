package com.seriouscatgames.couchlover.client;

import android.app.Activity;
import android.inputmethodservice.InputMethodService;
import android.inputmethodservice.Keyboard;
import android.os.Bundle;
import android.os.ResultReceiver;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.Window;
import android.view.inputmethod.InputMethodManager;
import android.widget.Toast;
import com.seriouscatgames.couchlover.client.net.*;

public class CouchLoverClientActivity extends Activity {
    /** Called when the activity is first created. */
	
	public TcpClient client;
	
	public int startX, startY;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);
        //this.requestWindowFeature(Window.FEATURE_NO_TITLE);
        startX = 0;
        startY = 0;
        
		try {
			client = new TcpClient("192.168.178.39", 666);
			client.Connect();
			Toast.makeText(getApplicationContext(), "Logged on", Toast.LENGTH_LONG).show();
		} catch(Exception e) {
			Toast.makeText(getApplicationContext(), e.getMessage(), Toast.LENGTH_LONG).show();
		}
    }
    
    @Override
    public boolean onTouchEvent(MotionEvent event) {
        int x = (int)event.getX();
        int y = (int)event.getY();
        switch (event.getAction()) {
            case MotionEvent.ACTION_DOWN:
            	startX = x;
            	startY = y;
            	break;
            case MotionEvent.ACTION_MOVE:
            	int deltaX = x - startX;
            	int deltaY = y - startY;
            	client.SendMessage("mouseMove:"+ deltaX + ";" + deltaY);
            	
            	startX = x;
            	startY = y;
            	
            	break;
            case MotionEvent.ACTION_UP:
            	client.SendMessage("mouseUp:"+ x + ";" + y);
            	break;
        }
        return false;
    }
        
    @Override
    public boolean onKeyUp(int keyCode, KeyEvent event) {
        if (keyCode == KeyEvent.KEYCODE_MENU) {
            InputMethodManager imm = (InputMethodManager) this.getSystemService(getApplicationContext().INPUT_METHOD_SERVICE);
            Toast.makeText(getApplicationContext(), "MENU PRESSED", Toast.LENGTH_LONG).show();
        }
        return true;
    }    

    public void rightClick(View view) {
        client.SendMessage("rightClick:");
    }
    
    public void leftClick(View view) {
    	client.SendMessage("leftClick:"); 
    }    
    
    
}