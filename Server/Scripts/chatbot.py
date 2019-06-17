# from chatterbot.trainers import ListTrainer
from chatterbot import ChatBot
from g2p_en import g2p
#import sys

#Important methods which are needed for lip sync
def p2vmap():
    roughMap = {
        ('AA0', 'AA1', 'AA2'): '1',
        ('AH0', 'AH1', 'AH2', 'HH'): '2',
        ('AO0', 'A01', 'A02'): '3',
        ('AW0', 'AW1', 'OW0', 'OW1', 'OW2'): '4',
        ('OY0', 'OY1', 'OY2', 'UH0', 'UH1', 'UH2', 'UW', 'UW0', 'UW1', 'UW2'): '5',
        ('EH0', 'EH1', 'EH2', 'AE0', 'AE1', 'AE2'): '6',
        ('IH0', 'IH1', 'IH2', 'AY0', 'AY1', 'AY2'): '7',
        ('EY0', 'EY1', 'EY2'): '8',
        ('Y', 'IY0', 'IY1', 'IY2'): '9',
        ('R', 'ER0', 'ER1', 'ER2'): '10',
        ('L'): '11',
        ('W'): '12',
        ('M', 'P', 'B'): '13',
        ('N', 'NG', 'DH', 'D', 'G', 'T', 'Z', 'ZH', 'TH', 'K', 'S'): '14',
        ('CH', 'JH', 'SH'): '15',
        ('F', 'V'): '16'
    }
    finalMap = {}
    for k, i in roughMap.items():
        for key in k:
            finalMap[key] = i

    return finalMap

def getViseme(grapheme):
    phoneme = g2p(grapheme)

    map = p2vmap()
    viseme = []
    vistring = ""

    for k in phoneme:
        if k in map:
            viseme.append(map[k])
            # prev=k
        else:
            viseme.append(-1)
    viseme.append(-1)

    for vis in viseme:
        vistring = vistring + str(vis) + " ";
    print(vistring)
    return vistring

#Modify this method if some other chatbot is to be used
def getResponse(req):
    bot=ChatBot('chatbot')
    # conv=open('C:\\Users\\ibm\\PycharmProjects\\Test\\chats.txt','r').readlines()
    # trainer =ListTrainer(bot)
    # trainer.train(conv)

    request=req

    response=bot.get_response(request)

    print(response)

    return response.text

