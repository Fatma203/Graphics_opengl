/*
 fatma mohmed salem 
  2021170388
  CS
level 3
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;


using System.IO;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Graphics
{
    class Renderer
    {
        Shader sh;



        uint xyzAxesBufferID;
        uint bodyBufferID;
        uint wheelBufferID;
        uint windowBufferID;
        uint floorBufferID;

        //3D Drawing
        mat4 ModelMatrix;
        mat4 ModelMatrix2;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;

        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationAngle = 0;

        public float translationX = 0,
                     translationY = 0,
                      translationZ = 3;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 carCenter;
        Texture floor_texture;
        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            Gl.glClearColor(0, 0, 0.4f, 1);

            floor_texture =new Texture(projectPath + "\\Textures\\Ground.jpg", 1);

            float[] xyzAxesVertices = {
		        //x
		        0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
		        100.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
		        //y
	            0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, //G
		        0.0f, 100.0f, 0.0f, 0.0f, 1.0f, 0.0f, //G
		        //z
	            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f,  //B
		        0.0f, 0.0f, 100.0f, 0.0f, 0.0f, 1.0f,  //B
            };

            float[] bodyVertices = { 
    // Front face
    -1.5f, -1.0f, 1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 0
    1.5f, -1.0f, 1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 1
    1.5f, 1.0f, 1.0f,       1.0f, 0.0f, 0.0f,  // Vertex 2
    1.5f, 1.0f, 1.0f,       1.0f, 0.0f, 0.0f,  // Vertex 3
    -1.5f, 1.0f, 1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 4
    -1.5f, -1.0f, 1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 5

    // Back face
    -1.5f, -1.0f, -1.0f,    1.0f, 0.0f, 0.0f,  // Vertex 6
    1.5f, -1.0f, -1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 7
    1.5f, 1.0f, -1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 8
    1.5f, 1.0f, -1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 9
    -1.5f, 1.0f, -1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 10
    -1.5f, -1.0f, -1.0f,    1.0f, 0.0f, 0.0f,  // Vertex 11

    // Top face
    -1.5f, 1.0f, 1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 12
    1.5f, 1.0f, 1.0f,       1.0f, 0.0f, 0.0f,  // Vertex 13
    1.5f, 1.0f, -1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 14
    1.5f, 1.0f, -1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 15
    -1.5f, 1.0f, -1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 16
    -1.5f, 1.0f, 1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 17

    // Bottom face
    -1.5f, -1.0f, 1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 18
    1.5f, -1.0f, 1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 19
    1.5f, -1.0f, -1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 20
    1.5f, -1.0f, -1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 21
    -1.5f, -1.0f, -1.0f,    1.0f, 0.0f, 0.0f,  // Vertex 22
    -1.5f, -1.0f, 1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 23

    // Right face
    1.5f, -1.0f, 1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 24
    1.5f, -1.0f, -1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 25
    1.5f, 1.0f, -1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 26
    1.5f, 1.0f, -1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 27
    1.5f, 1.0f, 1.0f,       1.0f, 0.0f, 0.0f,  // Vertex 28
    1.5f, -1.0f, 1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 29

    // Left face
    -1.5f, -1.0f, 1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 30
    -1.5f, -1.0f, -1.0f,    1.0f, 0.0f, 0.0f,  // Vertex 31
    -1.5f, 1.0f, -1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 32
    -1.5f, 1.0f, -1.0f,     1.0f, 0.0f, 0.0f,  // Vertex 33
    -1.5f, 1.0f, 1.0f,      1.0f, 0.0f, 0.0f,  // Vertex 34
    -1.5f, -1.0f, 1.0f   ,   1.0f, 0.0f, 0.0f   // Vertex 35
};
            float[] wheelVertices = { 
                //first one
            -0.75f,1.0f,-1.0f, 0.0f, 0.0f, 0.0f,
            -1.5f,1.0f,-1.0f,  0.0f, 0.0f, 0.0f,
            -1.5f,1.0f,-0.5f,  0.0f, 0.0f, 0.0f,
            -0.75f,1.0f,-0.5f, 0.0f, 0.0f, 0.0f,

            //second one 
             1.5f,1.0f,-1.0f,  0.0f, 0.0f, 0.0f,
              0.75f,1.0f,-1.0f, 0.0f, 0.0f, 0.0f,
              0.75f,1.0f,-0.5f, 0.0f, 0.0f, 0.0f,
               1.5f,1.0f,-0.5f,  0.0f, 0.0f, 0.0f,

               //third one
               0.75f,-1.0f,-1.0f,  0.0f, 0.0f, 0.0f,
               1.5f,-1.0f,-1.0f,  0.0f, 0.0f, 0.0f,
               1.5f,-1.0f,-0.5f,  0.0f, 0.0f, 0.0f,
               0.75f,-1.0f,-0.5f,  0.0f, 0.0f, 0.0f,

               //fourth one
               -1.5f,-1.0f,-0.5f,  0.0f, 0.0f, 0.0f,
                -1.5f,-1.0f,-1.0f,  0.0f, 0.0f, 0.0f,
                - 0.75f,-1.0f,-1.0f,  0.0f, 0.0f, 0.0f,
                - 0.75f,-1.0f,-0.5f,  0.0f, 0.0f, 0.0f,
            };

            float[] windowVertices = { 
             //first window 
             -1.5f,1.0f,1.0f,  1.0f, 1.0f, 1.0f,
             -1.5f,1.0f,0.25f,  1.0f, 1.0f, 1.0f,
              -1.5f,0.25f,0.25f,  1.0f, 1.0f, 1.0f,
              -1.5f,0.25f,1.0f,  1.0f, 1.0f, 1.0f,

              //second window 
               -1.5f,-0.25f,1.0f,  1.0f, 1.0f, 1.0f,
                -1.5f,-0.25f,0.25f,  1.0f, 1.0f, 1.0f,
                -1.5f,-1.0f,0.25f,  1.0f, 1.0f, 1.0f,
                 -1.5f,-1.0f,1.0f,  1.0f, 1.0f, 1.0f,


            };

            float[] floorverts = {
               // floor
            0.0f,0.0f,0.0f,     0.0f,0.0f,0.0f, 0,0,
            100.0f,0.0f,0.0f,     0.0f,0.0f,0.0f, 1,0,
            100.0f,100.0f,0.0f,    0.0f,0.0f,0.0f, 1,1,
            0.0f,100.0f,0.0f,     0.0f,0.0f,0.0f, 0,1,

            };





            //  triangleBufferID = GPU.GenerateBuffer(triangleVertices);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);
            bodyBufferID = GPU.GenerateBuffer(bodyVertices);

            wheelBufferID = GPU.GenerateBuffer(wheelVertices);
            windowBufferID = GPU.GenerateBuffer(windowVertices);
            floorBufferID=GPU.GenerateBuffer(floorverts);
            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(10, 10, 10), // Camera is at (0,5,5), in World Space
                        new vec3(0, 0, 0), // and looks at the origin
                        new vec3(0, 0, 1)  // Head is up (set to 0,-1,0 to look upside-down)
                );
            // Model Matrix Initialization
            ModelMatrix = new mat4(1);
            ModelMatrix2 = new mat4(1);
            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);

            // Our MVP matrix which is a multiplication of our 3 matrices 
            sh.UseShader();


            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            #region XYZ axis
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, xyzAxesBufferID);

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array()); // Identity

            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion
            // Draw the car body
            DrawCarBody();

            // Draw the car wheels
            DrawWheels();

            // Draw the car windows
            DrawWindows();

            //Draw the floor 
            DrawFloor();

            Gl.glFlush();

        }

        void DrawCarBody()
        {
            // Bind body buffer
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, bodyBufferID);

            // Set model matrix for car body
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            // Enable vertex attributes
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0); // Vertices
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float))); // Colors



            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_STRIP, 6, 6);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 12, 6);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 18, 6);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 24, 6);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 30, 6);

            // Disable vertex attributes
            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
        }

        void DrawWheels()
        {
            // Bind wheel buffer
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, wheelBufferID);

            // Set model matrix for wheels
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            // Enable vertex attributes
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0); // Vertices
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float))); // Colors


            Gl.glDrawArrays(Gl.GL_QUADS, 0, 4);
            Gl.glDrawArrays(Gl.GL_QUADS, 4, 4);
            Gl.glDrawArrays(Gl.GL_QUADS, 8, 4);
            Gl.glDrawArrays(Gl.GL_QUADS, 12, 4);


            // Disable vertex attributes
            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
        }


        void DrawWindows()
        {
            // Bind window buffer
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, windowBufferID);

            // Set model matrix for windows
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            // Enable vertex attributes
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0); // Vertices
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float))); // Colors


            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 4, 4);


            // Disable vertex attributes
            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
        }

        void DrawFloor()
        {
            // Bind window buffer
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, floorBufferID);

            // Set model matrix for windows
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix2.to_array());

            // Enable vertex attributes
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)0); // Vertices
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float))); // Colors

            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            //floor_texture.Bind();

            Gl.glDrawArrays(Gl.GL_QUADS, 0, 4);



            // Disable vertex attributes
            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            Gl.glDisableVertexAttribArray(2);
        }
        public void Update()
        {
            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds/1000.0f;
            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), -1 * carCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, 0, 1)));
            transformations.Add(glm.scale(new mat4(1), new vec3(1, 1, 1.5f)));
            transformations.Add(glm.translate(new mat4(1), carCenter));
            transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            ModelMatrix =  MathHelper.MultiplyMatrices(transformations);

            vec3 floorcenter = new vec3(0, 0, 0);
            List<mat4> floortransformations = new List<mat4> {
              glm.translate(new mat4(1), floorcenter)
            };
            ModelMatrix2 =  MathHelper.MultiplyMatrices(floortransformations);
            timer.Reset();
            timer.Start();
        }
        public void onkeyboardKeypress(char key)
        {
            float speed = 5;

            if (key == 'd')
                translationX += speed;
            if (key == 'a')
                translationX -= speed;

            if (key == 'w')
                translationY += speed;
            if (key == 's')
                translationY -= speed;

            if (key == 'z')
                translationZ += speed;
            if (key == 'c')
                translationZ -= speed;
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
