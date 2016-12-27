# CameraShake
An extensible, lightweight noise-based camera shake manager for Unity.

This implementation generates a concatenated camera matrix which allows for multiple camera shakes to seamlessly stack together. This also allows for easy integration with existing camera behavior scripts because it's not actually modifying the camera's transform.

It can be used for short violent shakes, subtle ambient shakes and everything in-between. All the shake properties can be quickly tuned in the inspector for fast iteration cycles.

### Noise Types
* Sin
* Perlin

How to Use
------
1. Download and open the project (Unity 5.4.1).
2. Attach a CameraShakeManager to your camera.
3. Create a few CameraShake resources and edit the properties to your liking.
4. Play & Stop the effects via CameraShakeManager as desired.

Example Screenshot
------
![Alt text](http://i.imgur.com/SYmmdND.png "Unity Editor Screenshot")

Example GIFs
------
![Alt text](http://i.imgur.com/0RRelTb.gif "Unity Editor GIF")
