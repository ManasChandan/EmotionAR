# -*- coding: utf-8 -*-
"""
Created on Sat Apr  4 13:45:50 2020

@author: Manas C Behera
"""

import os
import cv2
from PIL import Image
import numpy as np
recognizer = cv2.face.LBPHFaceRecognizer_create()
path = 'DataSets'

def getImageswithID(path):
    Imagespath = [os.path.join(path, f) for f in os.listdir(path)]
    faces = []
    IDs = []
    for imagepaths in Imagespath:
        faceImage  = Image.open(imagepaths).convert('L')
        faceNp = np.array(faceImage,'uint8')
        ID = int(os.path.split(imagepaths)[-1].split('.')[1])
        faces.append(faceNp)
        IDs.append(ID)
        print(ID)
        cv2.imshow('train',faceNp)
        cv2.waitKey(10)
    return np.array(IDs),faces

IDs,faces = getImageswithID(path)
recognizer.train(faces,IDs)
recognizer.save('Recognizer/mood_applicationNewData.yml')
cv2.destroyAllWindows()

        
        
        


