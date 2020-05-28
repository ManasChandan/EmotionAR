import cv2
from tensorflow.keras.models import load_model
import numpy as np
import time
from firebase import firebase

# Cascades
cascade_file = "haarcascade_frontalface_default.xml"
face_detection = cv2.CascadeClassifier(cascade_file)
recognizer = cv2.face.LBPHFaceRecognizer_create()
recognizer.read('Recognizer/moodNew_application.yml')

# Model Initials
model_file = "Expressio.h5"
emotions = ["angry","disgust","scared", "happy", "sad", "surprised","neutral"]
emotion_classifier = load_model(model_file, compile=False)
emotion,ide=0,0

# Firebase
fire = firebase.FirebaseApplication('https://moodapplication-be743.firebaseio.com/',None)

# Labels
trlabel,modlabel,malabel = 0,0,0
def get_photo():
    global trlabel,modlabel,malabel
    cap = cv2.VideoCapture(0)
    stat_time = time.time()
    while (True):
        ret, img = cap.read()
        faces = face_detection.detectMultiScale(img, 1.3, 5)
        for (x, y, w, h) in faces:
            detected_face = img[int(y):int(y + h), int(x):int(x + w)]
            cv2.rectangle(img, (x, y), (x + w, y + h), (0, 176, 24), 2)
            detected_face = cv2.cvtColor(detected_face, cv2.COLOR_BGR2GRAY)
            ide, conf = recognizer.predict(detected_face)
            if(ide == 1):
                name = 'Trump'
                trlabel = get_emotion(detected_face)
                label = trlabel
            elif(ide == 2):
                name = 'Modi'
                modlabel = get_emotion(detected_face)
                label = modlabel
            elif(ide == 3):
                name = 'Manas'
                malabel = get_emotion(detected_face)
                label = malabel
            cv2.putText(img, name, (x+w, y+int(h/4)), cv2.FONT_HERSHEY_SIMPLEX, 1, (42, 0, 202), 3, cv2.LINE_AA)
            cv2.putText(img, emotions[label], (x + w, y+int(2*h/4)), cv2.FONT_HERSHEY_SIMPLEX, 1, (42, 0, 202), 3, cv2.LINE_AA)
        cv2.imshow('',img)
        stop_time = time.time()
        elapsed = stop_time - stat_time
        if(elapsed > 20.00):
            stat_time = stop_time
            data_send(trlabel,modlabel,malabel)
            time.sleep(1)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

def get_emotion(face_array):
    detected_face = cv2.resize(face_array, (48, 48), interpolation=cv2.INTER_AREA)
    detected_face = np.array(detected_face).reshape((1, 48, 48, 1)) / 255.0
    label = np.argmax(emotion_classifier.predict(detected_face))
    newlabel = label.tolist()
    return newlabel

def data_send(tl,mol,mal):
    print('Updating Data Please Wait ........')
    fire.put("",'Modi',mol)
    fire.put("",'Manas',mal)
    fire.put("",'Trump',tl)
    print('Values updated')

get_photo()

