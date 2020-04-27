from ibm_watson import ToneAnalyzerV3
from ibm_cloud_sdk_core.authenticators import IAMAuthenticator

import os
import json
from dotenv import load_dotenv
from time import time

def toneAnalyser(text, server='https://api.eu-gb.tone-analyzer.watson.cloud.ibm.com'):
    TONE_API_KEY = os.getenv('TONEANALYSERAPI')
    tone_analyser = ToneAnalyzerV3(version='2020-04-27',authenticator=IAMAuthenticator(TONE_API_KEY))
    tone_analyser.set_service_url(server)
    tone_analysis = tone_analyser.tone( {'text': text}, content_type='application/json').get_result()
    #print(json.dumps(tone_analysis, indent=2))      # Data in - for debug
    
    documentTone = [(i["tone_name"],i["score"]) for i in tone_analysis["document_tone"]["tones"]]
    #sentenceTone = tone_analysis["sentences_tone"]  # Returns the tone for each sentence
   
    def sortTones(lst):
        lst.sort(key = lambda x: x[1], reverse=True)  
        return lst
    return sortTones(documentTone)

def main():
    
    #Example text
    text = 'Team, I know that times are tough! Product'\
           'sales have been disappointing for the past three '\
           'quarters. We have a competitive product, but we '\
           'need to do a better job of selling it!'

    documentTone = toneAnalyser(text)
    print("Document Tone:")    
    for i in documentTone:
        print(f"- {i[0]}, {i[1]}")


if __name__ == "__main__":    
    try:
        load_dotenv()
        main()
    except Exception as e:
        print("Error:", e)