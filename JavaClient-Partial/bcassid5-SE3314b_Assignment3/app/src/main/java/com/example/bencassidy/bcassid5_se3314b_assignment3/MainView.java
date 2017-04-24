package com.example.bencassidy.bcassid5_se3314b_assignment3;

import android.graphics.Bitmap;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.view.View;

import java.io.IOException;

public class MainView extends AppCompatActivity {

    Button btn_con;
    Button btn_py;
    Button btn_pe;
    Button btn_su;
    Button btn_tr;
    ImageView iv_image;
    TextView tv_port;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_view);

        btn_con = (Button) findViewById(R.id.btn_connect);
        btn_su = (Button) findViewById(R.id.btn_set);
        btn_py = (Button) findViewById(R.id.btn_play);
        btn_pe = (Button) findViewById(R.id.btn_pause);
        btn_tr = (Button) findViewById(R.id.btn_teardown);

        iv_image = (ImageView) findViewById(R.id.iv_display);

        tv_port = (TextView) findViewById(R.id.et_portNo);

        final Controller _control = new Controller();


        btn_con.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                //tv_port.setText("Hello");
                String portString = tv_port.getText().toString();
                int pn = Integer.parseInt(portString);
                try {
                    _control.connect(pn);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        });

        btn_su.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                try {
                    _control.setup();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        });
        btn_py.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                _control.play();
            }
        });
        btn_pe.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                _control.pause();
            }
        });
        btn_tr.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                _control.teardown();
            }
        });

    }

    public void setImage(Bitmap image) {
        this.iv_image.setImageBitmap(image);
    }

    public int getPort(){
        String portString = tv_port.getText().toString();
        int pn = Integer.parseInt(portString);
        return pn;
    }

    public void clearImage() {
        iv_image.setImageResource(R.drawable.black);
    }
}

