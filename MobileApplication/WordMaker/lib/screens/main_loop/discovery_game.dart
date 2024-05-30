import 'package:word_maker/screens/main_loop/game_tutorial.dart';
import 'package:word_maker/screens/profile_control/profile.dart';
import 'package:word_maker/screens/services/api_service.dart';
import 'package:word_maker/screens/services/db_service.dart';
import 'package:word_maker/screens/main_loop/home.dart';
import 'package:path_provider/path_provider.dart';
import 'package:hive_flutter/hive_flutter.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:url_launcher/url_launcher.dart';
import 'package:diacritic/diacritic.dart';
import 'package:flutter/material.dart';
import 'package:lottie/lottie.dart';
import 'dart:convert';
import 'dart:async';
import 'dart:math';
import 'dart:io';

File? imgFile;
int points = 0;
String selectedWord = '', wordGuess = '', _word = '';
Map<String, String> textMap = {}, resetList = {};
List<String> filteredList = [];
Map<String, List<String>> wordList = {};
int _wordCount = 0, wordLength = 0, _totalTimerSeconds = 0;
Map<String, dynamic> loadStat = {}, userData = {};
List<Widget>? listWidgets = [];
String timerString = '00:00';
bool _levelCompleted = false;
BuildContext? gridContext;

class DiscoveryGame extends StatefulWidget {
  final int wordLevel;
  final Map<String, dynamic> userData;
  final Box dataBox;
  const DiscoveryGame({super.key, required this.wordLevel, required this.dataBox, required this.userData,});

  @override
  State<DiscoveryGame> createState() => _DiscoveryGameState();
}

List<Widget> findAllWidgetsByType<T>(Element? element) {
  List<Widget> foundWidgets = [];

  void searchForWidgets(Element? currentElement) {
    if (currentElement == null) return;

    if (currentElement.widget is T) {
      foundWidgets.add(currentElement.widget);
    }
    currentElement.visitChildren(searchForWidgets);
  }
  searchForWidgets(element);
  return foundWidgets;
}

class _DiscoveryGameState extends State<DiscoveryGame> with SingleTickerProviderStateMixin{
  Color _answerColor = Colors.black;
  List<Offset> initialPositions = [];
  List<String> letterList = [];
  List<Offset> positions = [];
  late Future _future;
  late final AnimationController _controller;

  void showSnackBar(deviceHeight, deviceWidth) {
    loadStat = loadWordStats(userData['User'], widget.wordLevel);

    final snackBar = SnackBar(
      backgroundColor: Colors.blue[900],
      elevation: 5,
      content: Column(
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Column(
                children: [
                  Text(
                    timerString,
                    style: TextStyle(
                      fontSize: deviceHeight * 0.03,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  Text(
                    'Time elapsed', 
                    style: TextStyle(
                      fontSize: deviceHeight * 0.02
                    ),
                  ),
                ],
              ),
              SizedBox(width: deviceWidth * 0.1,),
              Column(
                children: [
                  Text(
                    '${loadStat['Score']}%',
                    style: TextStyle(
                      fontSize: deviceHeight * 0.03,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  Text(
                    'Score', 
                    style: TextStyle(
                      fontSize: deviceHeight * 0.02
                    ),
                  ),
                ],
              ),
            ],
          ),
          SizedBox(height: deviceHeight * 0.01,),
          ElevatedButton(
            onPressed: (){
              ScaffoldMessenger.of(context).hideCurrentSnackBar();
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => DiscoveryGame(wordLevel: widget.wordLevel + 1, userData: userData, dataBox: widget.dataBox,),)
              );
            },
            style: ButtonStyle(
              minimumSize: WidgetStatePropertyAll(Size(deviceWidth* 0.2, deviceHeight * 0.05)),
              backgroundColor: const WidgetStatePropertyAll(Colors.deepPurple),
              shape: WidgetStatePropertyAll(
                RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(50),
                  side: const BorderSide(
                    color: Colors.white,
                    width: 2
                  ),
                ),
              ),
            ), 
            child: Text(
              'Next Unsolved Puzzle',
              style: TextStyle(
                fontSize: deviceHeight * 0.02,
                color: Colors.white,
              ),
            ),
          ),
        ],
      ),
      duration: const Duration(days: 365), 
    );
    
    ScaffoldMessenger.of(context).showSnackBar(snackBar);
  }

  String getMyTextKeys(BuildContext context) {
    Element? rootElement = Navigator.of(context).context as Element?;

    List<Widget> textWidgets = findAllWidgetsByType<Text>(rootElement);
    if (textWidgets.isNotEmpty) {
      for (var textWidget in textWidgets) {
        Key? textKey =  textWidget.key;
        if (textKey != null) {    
          if (removeDiacritics(textKey.toString().toLowerCase()) == '[<\'$wordGuess\'>]') {
            if(!textMap.containsValue(wordGuess.substring(0, 1).toUpperCase() + wordGuess.substring(1))) {
              setState(() {
                String newText = textKey.toString().replaceAll('[<\'', ' ').replaceAll('\'>]', ' ').trim();
                String newValue = newText.substring(0, 1).toUpperCase() + newText.substring(1);
                textMap.addAll({newText:newValue});
                wordFound(true);
              });
            }
            return textKey.toString();
          }
        }
      }
    }
    return '';
  }

  void saveWordStats(Map<String, dynamic> dataMap) {
    try {
      for (var key in dataMap.keys) {
        widget.dataBox.put('Word${widget.wordLevel}:$key', dataMap[key]);
      }
    } catch (e) {
        throw 'Error while saving Word#${widget.wordLevel}: $e';
      }
  }  
  
  Map<String, dynamic> loadWordStats(user, wordLevel) {
    try {
      Map<String, dynamic> loadList = {
        'Count':widget.dataBox.get('Word$wordLevel:Count',  defaultValue: 0), 
        'Word':widget.dataBox.get('Word$wordLevel:Word',  defaultValue: ''), 
        'WordList':widget.dataBox.get('Word$wordLevel:WordList', defaultValue: []),
        'Timer':widget.dataBox.get('Word$wordLevel:Timer',  defaultValue: '00:00'), 
        'Score':widget.dataBox.get('Word$wordLevel:Score',  defaultValue: '100'), 
        'WordMap':widget.dataBox.get('Word$wordLevel:WordMap', defaultValue: {}), 
        'isCompleted':widget.dataBox.get('Word$wordLevel:isCompleted',  defaultValue: false), 
      };
      return loadList;
    } catch (e) {
      throw 'Error while loading Word$wordLevel data: $e';
    }
  }

  Future fetchApi(maxLength, language) async {
    loadStat = loadWordStats(userData['User'], widget.wordLevel);
  
    if (loadStat['Word'] == '') {
      filteredList.clear();
      wordList.clear();

      final response = await Api().callApi(maxLength, language);
       
      if (response.statusCode == 200) {
        final apiData = jsonDecode(response.body);

        for (var item in apiData) {
          String word = item['word'] ?? 'NULL';
          String definition = item['definition'] ?? 'NULL';
          String example = item['example'] ?? 'NULL';
          String sourceUrl = item['sourceUrl'] ?? 'NULL';
          
          filteredList.add(word);
          wordList[word] = [definition, example, sourceUrl];
        }
      } else {
        throw Exception('Failed to load API data');
      }
    }
  }
  
  Future<void> apiResult() async{
    if (widget.wordLevel <= 20) {
      wordLength = 5;
      points = 50;
    } else if (widget.wordLevel > 20 && widget.wordLevel <= 45) {
      points = 75;
      wordLength = 6;
    } else {
      points = 100;
      wordLength = 7;
    }

    _future = fetchApi(wordLength, 'English').then((_) => loadData());
  }

  void loadData() async {
    final directory = await getApplicationDocumentsDirectory();
    final path = directory.path;
    final imagePath = '$path/profile_img.png'; 

    try {
      loadStat = loadWordStats(userData['User'], widget.wordLevel);
      textMap.clear();
      wordGuess = '';

      if (loadStat['Word'] == ''){
        _levelCompleted = false;
        _wordCount = 0;
        selectedWord = filteredList.first;   
        letterList = selectedWord.toLowerCase().split('').toSet().toList();
        saveWordStats({'Count':_wordCount, 'Word':selectedWord, 'WordMap': jsonEncode(textMap), 'WordList':filteredList, 'isCompleted':false});
      }
      else {
        filteredList = List.from(loadStat['WordList']);
        letterList = loadStat['Word'].toString().toLowerCase().split('').toSet().toList();
        _levelCompleted = loadStat['isCompleted'];
        timerString = loadStat['Timer'];
        _wordCount = loadStat['Count'];
        textMap = Map<String, String>.from(jsonDecode(loadStat['WordMap']));
      }
      
      if(_levelCompleted){
        setState(() {
          timerString = loadStat['Timer'];
          wordGuess = loadStat['Word'];
          showSnackBar(MediaQuery.of(context).size.height, MediaQuery.of(context).size.width);
        });
      }

      filteredList.sort((a, b) => a.length.compareTo(b.length));
      loadStat['Word'] == '' ? _word = selectedWord : _word = loadStat['Word']; 
      _generatePositions();
      letterList.shuffle();
      listWidgets?.clear();
      saveWordStats({'Length':filteredList.length});     

      imgFile = File(imagePath);
      
      if (loadStat['Timer'] != '00:00') {
        List<String> time = loadStat['Timer'].toString().split(":");
        int minutes = int.parse(time[0]);
        int seconds = int.parse(time[1]);
        _totalTimerSeconds = minutes * 60 + seconds;
      } else {
        _totalTimerSeconds = 0;
      }
      setState(() {});

    } catch (e) {
      throw 'Error trying to load user data: $e';
    }
  }

  Widget confettiAnimation() {
    return IgnorePointer(
      child: Lottie.asset(
          'assets/animations/confetti.json',
          controller: _controller,
          height: MediaQuery.sizeOf(context).height,
          width: MediaQuery.sizeOf(context).width,
          fit: BoxFit.cover,
          repeat: false,
        ),
    );
  }

  @override
  void initState() {
    super.initState();
    userData = widget.userData;
    _controller = AnimationController(vsync: this, duration: const Duration(seconds: 3));
    apiResult();
  }

  @override
  void dispose() {
    super.dispose();
    _controller.dispose();
    textMap.clear();
  }

  Future<void> _launchURL(String url) async {
    Uri uri = Uri.parse(url);
    if (await canLaunchUrl(uri)) {
      await launchUrl(uri, mode: LaunchMode.externalApplication,);
    } else {
      throw 'Error opening website: $uri. Try again later.';
    }
  }

  void _showMessage(word, desc, ex, url, deviceHeight, deviceWidth) {
    word = word.toString().substring(0, 1).toUpperCase() + word.toString().substring(1);
    showDialog(
      context: context, 
      builder: (context) {
        return AlertDialog(
          backgroundColor: Colors.grey[200],
          alignment: Alignment.center,
          title: Text(word, textAlign: TextAlign.center,),
          titleTextStyle: TextStyle(
            color: Colors.black, 
            fontSize: deviceHeight * 0.035,
            fontFamily: 'Arial',
            fontWeight: FontWeight.bold,
            decoration: TextDecoration.underline,
            decorationThickness: 1.2,
          ),
          content: Column(
            crossAxisAlignment: CrossAxisAlignment.center,
            mainAxisSize: MainAxisSize.min,
            children: [ 
              SizedBox(height: deviceHeight * 0.03,),
              Text(desc, textAlign: TextAlign.justify,),
              SizedBox(height: deviceHeight * 0.04,),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Text(
                    'Example using ', 
                    style: TextStyle(
                      fontSize: deviceHeight * 0.025, 
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  Text(
                    word,
                    style: TextStyle(
                      fontSize: deviceHeight * 0.025, 
                      fontWeight: FontWeight.w900,
                      color: Colors.red[900],
                    ),
                  )
                ]
              ),
              SizedBox(height: deviceHeight * 0.01,),
              Text(ex),
              SizedBox(height: deviceHeight * 0.05,),
              url != 'NULL' && url != null ? 
              Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  const Row(children: [Text('For additional information:')]),
                  TextButton(                        
                    onPressed: (){_launchURL(url);}, 
                    child: Center(
                      child: Text('Tap Here', 
                        style: TextStyle(
                          color: Colors.blue,
                          fontSize: deviceHeight * 0.025,
                          fontWeight: FontWeight.bold,
                          decoration: TextDecoration.underline,
                          decorationColor: Colors.blue
                        ),
                      ),
                    ),
                  ),                  
                ] 
              ) : Container(),
            ]
          ),
          contentTextStyle: TextStyle(
            color: Colors.black, 
            fontSize: deviceHeight * 0.025
          ),
          contentPadding: EdgeInsets.symmetric(
            horizontal: deviceWidth * 0.08,
            vertical: deviceHeight * 0.01,
          ),
          actions: [
            TextButton(
              onPressed: () {Navigator.of(context).pop();}, 
              child: Text(
                'OK',
                style: TextStyle(
                  fontSize: deviceHeight * 0.03,
                  color: Colors.black,
                ),
              ),
            ),
          ],
          actionsAlignment: MainAxisAlignment.end,
        );
      }
    );
  }

  void _generatePositions(){
    initialPositions = List.generate(letterList.length, (index) {
      final angleMath = (2 * pi * index) /  letterList.length;
      final angle =  angleMath + 1;
      final x = 80 * cos(angle);
      final y = 80 * sin(angle);
      return Offset(x, y);
    });

    positions = initialPositions.toList();
  }

  void revealLetter() {
    setState(() {
      loadStat = loadWordStats(userData['User'], widget.wordLevel);
      double.parse(loadStat['Score']) <= 0 ? saveWordStats({'Score': '0'}) : saveWordStats({'Score': (double.parse(loadStat['Score']) - 2.75).toString()});
    
      Random random = Random();
      var incompleteWords = textMap.entries.where((entry) => entry.value.contains('_')).toList();
      if (incompleteWords.isEmpty) return;

      var wordEntry = incompleteWords[random.nextInt(incompleteWords.length)];
      var word = wordEntry.key;
      var revealed = wordEntry.value.split(' ');


      for (var i = 0; i < word.length; i++) {
        if (revealed[i] == '_') {
          revealed[i] = word[i];
          break;
        }
      }

      if (!revealed.contains('_')) {
        textMap[word] = revealed.join('');
        wordFound(true);
      } else {
        textMap[word] = revealed.join(' ');
        wordFound(false);
      }
    });
  }

  void revealWord() {
    setState(() {
      loadStat = loadWordStats(userData['User'], widget.wordLevel);
      double.parse(loadStat['Score']) <= 0 ? saveWordStats({'Score': '0'}) : saveWordStats({'Score': (double.parse(loadStat['Score']) - 12.5).toString()});
      
      Random random = Random();
      var incompleteWords = textMap.entries.where((entry) => entry.value.contains('_')).toList();
      if (incompleteWords.isEmpty) return;

      var wordEntry = incompleteWords[random.nextInt(incompleteWords.length)];
      textMap[wordEntry.key] = wordEntry.key[0].toUpperCase() + wordEntry.key.substring(1);
      wordFound(true);
    });
  }

  void resetLevel() {
    if (loadStat['isCompleted']) {
      ScaffoldMessenger.of(context).hideCurrentSnackBar();
    }

    setState(() {
      textMap.clear();
      Map<String, dynamic> resetLevel = {
        'Count' : 0,
        'Timer' : '00:00',
        'Score' : '100',
        'WordMap' : textMap,
        'isCompleted' : false 
      };
      saveWordStats(resetLevel);
    });
    loadData();
  }

  void wordFound(saveWord) {
    if (saveWord) {
      Database db = Database();
      _wordCount += 1;

      if(_wordCount >= filteredList.length) {
        _levelCompleted = true;
        wordGuess = selectedWord;
        saveWordStats({'isCompleted':true});
        showSnackBar(MediaQuery.of(context).size.height, MediaQuery.of(context).size.width);
        var ticker = _controller.forward();
        ticker.whenComplete(() {
          _controller.reset();
        },);
      }

      saveWordStats({'Count':_wordCount, 'WordMap': jsonEncode(textMap), 'Timer':timerString});

      db.addPoints(userData['ID'], points).then((result) {
        setState(() {
          userData = {
            'ID': result[0][0],
            'User': result[0][1],
            'Email': result[0][2],
            'Level': result[0][3],
            'Points': result[0][4]
          };
        }); 
      });
    } else {
      saveWordStats({'WordMap': jsonEncode(textMap), 'Timer':timerString});
    }

  }

  void getContext(BuildContext context){
    gridContext = context;
  }
  
  void showBackDialog() {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        var height = MediaQuery.sizeOf(context).height;

        return AlertDialog(
          title: Text(
            'Leaving Level!',
            style: TextStyle(
              fontSize: height * 0.03,
              color: Colors.black,
            ),
          ),
          content: Text(
            'Are you sure you want to leave this level?',
            style: TextStyle(
              fontSize: height * 0.025
            ),
          ),
          actions: [
            Row(
              children: [
                TextButton(
                  child: Text(
                    'Leave',
                    style: TextStyle(
                      fontSize: height * 0.025,
                      color: Colors.black
                    ),
                  ),
                  onPressed: () {
                    ScaffoldMessenger.of(context).hideCurrentSnackBar();
                    saveWordStats({'Timer':timerString});
                    Navigator.pushReplacement(context, 
                      MaterialPageRoute(
                        builder: (context) => HomeScreen(dataBox: widget.dataBox, userData: userData,),
                      )
                    );
                  },
                ), 
                SizedBox(width: height * 0.12,),
                TextButton(
                  child: Text(
                    'Cancel',
                    style: TextStyle(
                      color: Colors.black,
                      fontSize: height * 0.025,
                    ),
                  ),
                  onPressed: () {
                    Navigator.pop(context);
                  },
                ), 
              ],
            ),
          ],
        );
      },
    );
  }

  void loadImage() async {
    final Directory appDir = await getApplicationDocumentsDirectory();
    final String appDirPath = appDir.path;
    final File imageFile = File('$appDirPath/profile_image.png');

    if (await imageFile.exists()) {
      setState(() {
        imgFile = imageFile;
      });
    }
  }

  List<String> filterWords(List<String> wordList, List<String> letterList) {
    return wordList.where((word) {
      return word.toLowerCase().split('').every((letter) => letterList.contains(letter.toLowerCase()));
    }).toList();
  }

  List<Widget> createTextWidgets(height, width) {
    String value;
    
    return filteredList.map((word) {
      if (textMap[word] == null){
        value = word.split('').map((_) => '_').join(' ');
      }else{
        value = textMap[word].toString().substring(0, 1).toUpperCase() + textMap[word].toString().substring(1);
      }

      setState(() {
        textMap.addAll({word:value});
      });
      
      return Center(
        child: InkWell(
          splashColor: Colors.transparent,
          highlightColor: Colors.transparent,
          onTap: () {
            if (word == textMap[word]!.toLowerCase()) {
              _showMessage(word, wordList[word]?[0] != '' && wordList[word]?[0] != 'NULL' && wordList[word]?[0] != null ? wordList[word]![0] : 'No definition was found for $word', wordList[word]?[1] != '' && wordList[word]?[1] != 'NULL' && wordList[word]?[1] != null ? wordList[word]![1] : 'No examples were found for $word',  wordList[word]?[2] != '' && wordList[word]?[2] != 'NULL' && wordList[word]?[2] != null ? wordList[word]![2] : '', height, width);
            }
          },
          child: Text(
            value,
            key: Key(word),
            style: TextStyle(
              color: Colors.black,
              fontWeight: FontWeight.bold,
              fontSize: height * 0.03,
            ),
          )
        ),
      );
    }).toList();
  }

  @override
  Widget build(BuildContext context) {
    getContext(context);
    listWidgets?.clear();
    listWidgets?.addAll(createTextWidgets(MediaQuery.of(context).size.height, MediaQuery.of(context).size.width));
    
    void updateTextWidgets(double height, double width) {
      setState(() {
        listWidgets?.clear();
        listWidgets?.addAll(createTextWidgets(height, width));
      });
    }

    void refreshLetters(){
      setState(() {      
        positions.shuffle();
      });
    }

    Widget appBody() {
      return PopScope( 
        canPop: false,
        onPopInvoked: (bool didPop) {
          if (didPop) return;
          showBackDialog();
        },
        child:LayoutBuilder(
          builder: (context, constraints) {
            double deviceWidth = constraints.maxWidth;
            double deviceHeight = constraints.maxHeight;
            return Scaffold(
              drawerEnableOpenDragGesture: true,
              appBar: AppBar(
                backgroundColor: Colors.blue[800],
                foregroundColor: Colors.white,
                toolbarHeight: deviceHeight * 0.095,
                title: Column (
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Word Maker', 
                      style: GoogleFonts.caveat(
                        textStyle: TextStyle(
                          fontSize: deviceHeight * 0.04,
                        ) 
                      )
                    ),
                    SizedBox(width: deviceWidth * 0.18,),
                    Row(
                      children: [
                        Text(
                          'Word #${widget.wordLevel} ',
                          style: TextStyle(
                            fontSize: deviceHeight * 0.02,
                          ),
                        ),
                        TimerWidget(deviceHeight: deviceHeight,),
                      ],
                    ),
                  ]
                ),
                actions: [
                  Theme(
                    data: Theme.of(context).copyWith(
                      popupMenuTheme: PopupMenuThemeData(
                        color: Colors.lightBlueAccent.shade100,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(20),
                          side: BorderSide(color: Colors.blue.shade900)
                        )
                      ),
                    ),
                    child: PopupMenuButton(
                      tooltip: 'Show Hints',
                      offset: Offset(-deviceWidth * 0.105, 0),
                      position: PopupMenuPosition.over,
                      icon: Icon(
                        color: Colors.white,
                        Icons.lightbulb_outline_rounded,
                        size: deviceHeight * 0.035,
                      ),
                      onSelected: (String value) {
                        if (value == 'Reveal Letter') {
                          revealLetter();
                        } else if (value == 'Reveal Word') {
                          revealWord();
                        } else if (value == 'Reset Level') {
                          showDialog(
                            context: context, 
                            builder: (context) {
                              return AlertDialog(
                                title: Text(
                                  'Resetting Level!',
                                  style: TextStyle(
                                    fontSize: deviceHeight * 0.03,
                                    color: Colors.black,
                                  ),
                                ),
                                content: Text(
                                  'Are you sure you want to reset this level?',
                                  style: TextStyle(
                                    fontSize: deviceHeight * 0.025
                                  ),
                                ),
                                actions: [
                                  Row(
                                    children: [
                                      TextButton(
                                        child: Text(
                                          'Yes',
                                          style: TextStyle(
                                            fontSize: deviceHeight * 0.025,
                                            color: Colors.black
                                          ),
                                        ),
                                        onPressed: () {
                                          Navigator.pop(context);
                                          resetLevel();
                                        },
                                      ), 
                                      SizedBox(width: deviceHeight * 0.165,),
                                      TextButton(
                                        child: Text(
                                          'No',
                                          style: TextStyle(
                                            color: Colors.black,
                                            fontSize: deviceHeight * 0.025,
                                          ),
                                        ),
                                        onPressed: () {
                                          Navigator.pop(context);
                                        },
                                      ), 
                                    ],
                                  ),
                                ],
                              ); 
                            },
                          );
                        }
                      },
                      itemBuilder: (BuildContext context) => <PopupMenuEntry<String>> [
                        PopupMenuItem(
                          value: 'Reveal Letter',
                          child: Center(
                            child: Text(
                              'Reveal Letter',
                              style: TextStyle(
                                fontSize: deviceHeight * 0.0235,
                              ),
                            ),
                          ),
                        ),
                        PopupMenuItem(
                          value: 'Reveal Word',
                          child: Center(
                            child: Text(
                              'Reveal Word',
                              style: TextStyle(
                                fontSize: deviceHeight * 0.0235,
                              ),
                            ),
                          ),
                        ),
                        PopupMenuItem(
                          height: deviceHeight * 0.025,
                          child: Divider(
                            color: Colors.black,
                            height: deviceHeight * 0.002,
                          ), 
                        ),
                        PopupMenuItem(
                          value: 'Reset Level',
                          child: Center(
                            child: Text(
                              'Reset Level',
                              style: TextStyle(
                                fontSize: deviceHeight * 0.0235,
                              ),
                            ), 
                          ) 
                        ),
                      ],
                    ),
                  ),
                  IconButton(
                    tooltip: 'How do I play?',
                    onPressed: (){
                      Navigator.push(context, 
                      MaterialPageRoute(builder: (context) => const TutorialScreen(),));
                    }, 
                    icon: Icon(
                      Icons.help_outline,
                      size: deviceHeight * 0.035,
                    ),
                  ),
                ],
              ),
              onDrawerChanged: (isOpened) {
                if (isOpened) {
                  loadImage();
                }
              },
              drawer: Drawer(
                width: deviceWidth * 0.7,
                surfaceTintColor: Colors.lightBlue,
                child: SingleChildScrollView(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: [
                      // Drawer Header
                      Container(
                        color: Colors.blue,
                        padding: EdgeInsets.only(top: deviceHeight * 0.075),
                        child:  Column(
                          children: [    
                            Hero(
                              tag: 'profileImage', 
                              child: CircleAvatar(
                                radius: deviceHeight * 0.08,
                                backgroundImage: imgFile != null && imgFile!.existsSync() ? Image.file(imgFile!).image : const AssetImage('assets/images/no_image.png'),
                              ),
                            ),                    
                            SizedBox(height: deviceHeight * 0.01),
                            Text(
                              userData['User'].toString().substring(0,1).toUpperCase() + userData['User'].toString().substring(1),
                              style: TextStyle(
                                fontSize: deviceHeight * 0.035,
                                color: Colors.white,
                              ),
                            ),
                            Text(
                              userData['Email'],
                              style: TextStyle(
                                fontSize: deviceHeight * 0.02,
                                color: Colors.white,
                              ),
                            ),
                            SizedBox(height: deviceHeight * 0.01,),
                            Divider(thickness: 2,endIndent: deviceHeight * 0.03,indent: deviceHeight * 0.03,),
                            SizedBox(height: deviceHeight * 0.01,),
                            Text('Level: ${userData['Level']}',
                              style: TextStyle(
                                fontSize: deviceHeight * 0.03,
                                color: Colors.white,
                              ),
                            ),
                            Text('Points: ${userData['Points']}',
                              style: TextStyle(
                                fontSize: deviceHeight * 0.03,
                                color: Colors.white,
                              ),
                            ),
                            SizedBox(height: deviceHeight * 0.01,)
                          ],
                        ),
                      ),
                      Container(
                        padding: EdgeInsets.symmetric(vertical: deviceHeight * 0.05, horizontal: deviceHeight * 0.02),
                        child: Wrap(
                          runSpacing: deviceHeight * 0.016,
                          children: [
                            ListTile(
                              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(30)),
                              leading: Icon(Icons.home_outlined, size: deviceHeight * 0.04,),
                              title: Text(
                                'Home',
                                style: TextStyle(
                                  fontSize: deviceHeight * 0.028,
                                ),
                              ),
                              onTap: (){
                                showBackDialog();
                              },
                            ),
                            ListTile(
                              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(30)),
                              leading: Icon(Icons.person_outline, size: deviceHeight * 0.04,),
                              title: Text(
                                'Profile',
                                style: TextStyle(
                                  fontSize: deviceHeight * 0.028,
                                ),
                              ),
                              onTap: (){
                                ScaffoldMessenger.of(context).hideCurrentSnackBar;
                                Navigator.pop(context);
                                Navigator.of(context).push( 
                                  MaterialPageRoute(
                                    builder: (context) => ProfileScreen(dataBox: widget.dataBox, userData: userData,),
                                  )
                                );
                              },
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              ),
              body: Stack(
                children: [
                  Container( 
                    decoration: const BoxDecoration(
                      gradient: LinearGradient(
                        colors: [Colors.lightBlueAccent, Colors.blueAccent, Colors.deepPurpleAccent, Colors.deepPurple],
                        begin: Alignment.topRight,
                        end: Alignment.bottomRight
                      ),
                    ),
                    child: Column( 
                      children: [ 
                        SizedBox(
                          height: deviceHeight * 0.25,
                          child: SizedBox.expand(
                            child: Center(
                              child: GridView.count(
                                shrinkWrap: true,
                                crossAxisCount: 3,
                                crossAxisSpacing: deviceHeight * 0.001,
                                mainAxisSpacing: deviceWidth * 0.001,
                                padding: EdgeInsets.symmetric(
                                  horizontal: deviceWidth * 0.045,
                                  vertical: deviceHeight * 0.03
                                ),
                                childAspectRatio: (deviceHeight / deviceWidth) + 0.5,
                                physics: const NeverScrollableScrollPhysics(),
                                children: listWidgets!.map((widget) {
                                  return widget;
                                }).toList(),
                              ),
                            ),
                          ),
                        ),                
                        SizedBox(height: deviceHeight * 0.05,),
                        Container(
                          padding: EdgeInsets.symmetric(horizontal:  deviceWidth * 0.03),
                          decoration: BoxDecoration(
                            border: Border.all(
                              color: _answerColor,
                              width: 2, 
                            )
                          ),
                          child: Text(
                            wordGuess == '' ?  _word.split('').map((_) => '_').join(' ') : wordGuess.toUpperCase(),
                            style: TextStyle(
                              fontSize:  deviceHeight * 0.04,
                              color: _answerColor,
                            ),
                          ),
                        ),
                        SizedBox(height: deviceHeight * .05,),
                        AbsorbPointer(
                          absorbing: _levelCompleted,
                          child: Column(
                            children: [
                              Row(
                                mainAxisAlignment: MainAxisAlignment.center,
                                children:[ 
                                  CustomButton(
                                    deviceHeight: deviceHeight,
                                    text: 'Erase', 
                                    onPressed: (){
                                      setState(() {
                                        if (wordGuess != '') {
                                          wordGuess = wordGuess.substring(0, wordGuess.length - 1);
                                        }
                                      });
                                    }
                                  ),
                                  SizedBox(width: deviceHeight * 0.01,),
                                  CustomButton(
                                    deviceHeight: deviceHeight,
                                    text: 'Enter',
                                    onPressed: (){
                                      String key = getMyTextKeys(gridContext!);
                                      if (key != '') {
                                        setState(() {
                                          _answerColor = Colors.green.shade800;
                                          Future.delayed(
                                            const Duration(milliseconds: 500),(){ 
                                            setState(() {
                                              _answerColor = Colors.black;
                                            });
                                          });
                                          wordGuess = '';
                                        });
                                      }
                                      else
                                      {
                                        _answerColor = Colors.red;
                                        Future.delayed(
                                          const Duration(milliseconds: 500),(){ 
                                          setState(() {
                                            _answerColor = Colors.black;
                                          });
                                        });
                                        wordGuess = '';
                                      }
                                      updateTextWidgets(deviceHeight, deviceWidth);
                                    }, 
                                  ),
                                ]
                              ),
                              Container(
                                height: deviceHeight * 0.285,
                                margin: EdgeInsets.symmetric(
                                  vertical: deviceHeight * 0.02,
                                ),
                                child: Center(
                                  child: Stack(
                                    alignment: Alignment.topCenter,
                                    children: [               
                                      Positioned(
                                        bottom: 2,
                                        child: Container(
                                          height: deviceHeight * 0.28,
                                          width: deviceWidth * 0.63,
                                          decoration: BoxDecoration(
                                            shape: BoxShape.circle,
                                            color: Colors.white.withAlpha(100),
                                          ),
                                          child: Stack(
                                            children: [
                                              ...List.generate(letterList.length, (index) {
                                                return AnimatedPositioned(
                                                  duration: const Duration(milliseconds: 250),
                                                  left: positions.isEmpty ? 0 : positions[index].dx + (deviceWidth * 0.235), 
                                                  top: positions.isEmpty ? 0 : positions[index].dy + (deviceHeight * 0.103),
                                                  child: TextButton(
                                                    key: ValueKey( letterList[index]),
                                                    style: ButtonStyle(
                                                      minimumSize: WidgetStateProperty.all(Size(deviceWidth * 0.15,  deviceHeight * 0.07)),
                                                    ),
                                                    onPressed: () {
                                                      setState(() {
                                                        wordGuess +=  letterList[index];
                                                      }); 
                                                    },
                                                    child: Text(
                                                      letterList[index].toUpperCase(),
                                                      style: TextStyle(
                                                        color: Colors.black,
                                                        fontSize:  deviceHeight * 0.045
                                                      ),
                                                    ),
                                                  ),
                                                );
                                              }),
                                              Center( 
                                                child:IconButton(
                                                  style: ButtonStyle(
                                                    backgroundColor: WidgetStateProperty.all(Colors.black26),
                                                    minimumSize: WidgetStateProperty.all(Size(deviceWidth * 0.15,  deviceHeight * 0.07)),
                                                  ),
                                                  icon: Icon(
                                                    size:  deviceHeight * 0.06,
                                                    color: Colors.white,
                                                    Icons.refresh_outlined,
                                                  ),
                                                  onPressed: () {
                                                    refreshLetters();
                                                  },
                                                ),
                                              ),
                                            ],
                                          ),
                                        ),
                                      ),
                                    ],
                                  ), 
                                ),
                              ),
                            ],
                          ),
                        ),
                      ]
                    ),
                  ),
                  confettiAnimation(),                    
                ],
              ) 
            ); 
          },
        ),
      );
    }

    return FutureBuilder(
      future: _future, 
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting){
          return Container(
            width: MediaQuery.sizeOf(context).width,
            height: MediaQuery.sizeOf(context).height,
            decoration: const BoxDecoration(
              gradient: LinearGradient(
                colors: [Colors.lightBlueAccent, Colors.blueAccent, Colors.deepPurpleAccent, Colors.deepPurple],
                begin: Alignment.topRight,
                end: Alignment.bottomRight
              ),
            ),
            child: Center(
              child: SizedBox(
                height: MediaQuery.sizeOf(context).height * 0.1, 
                width: MediaQuery.sizeOf(context).height * 0.1,
                child: const CircularProgressIndicator(
                  color: Colors.white,
                ),
              ),
            ),
          );
        } else if (snapshot.hasError) {
          return AlertDialog(
            title: const Text(
              'Sorry, but it looks like our servers are down. \nTry again later.'
            ),
            actions: [
              ElevatedButton(
                onPressed: () {
                  Navigator.pushReplacement(
                    context,
                    MaterialPageRoute(builder: (context) => HomeScreen(dataBox: widget.dataBox, userData: userData),
                  ));
                },
                child: const Text('OK')
              ),
            ],
          );
        } else {
          return appBody();
        }

      }
    );
  }
}

// Custom Button
class CustomButton extends StatelessWidget {
  final String text;
  final VoidCallback onPressed;
  final double deviceHeight;

  const CustomButton({super.key, required this.text, required this.onPressed, required this.deviceHeight,});
  
  @override
  Widget build(BuildContext context) {
    return ElevatedButton(
      onPressed: onPressed,
      style: ElevatedButton.styleFrom(
        padding: EdgeInsets.symmetric(vertical: deviceHeight * 0.012, horizontal: deviceHeight * 0.025),
        backgroundColor: Colors.white.withAlpha(150),
      ),
      child: Text(
        text,
        style: TextStyle(
          fontSize: deviceHeight * 0.025,
          color: Colors.black,
        ),
      ),
    );
  }
}

class TimerWidget extends StatefulWidget {
  final double deviceHeight;
  const TimerWidget({super.key, required this.deviceHeight});

  @override
  State<TimerWidget> createState() => _TimerWidgetState();
}

class _TimerWidgetState extends State<TimerWidget> {
  Duration duration = const Duration();
  Timer? timer;
  
  @override
  void initState() {
    super.initState();
    
    if (!_levelCompleted){
      startTimer();
    }
  }

  @override
  void dispose() {
    timer?.cancel();
    super.dispose();
  }

  void addTime() {
    if(!_levelCompleted){
      String twoDigits(int n) => n.toString().padLeft(2, '0');
      final minutes = twoDigits(duration.inMinutes.remainder(60));
      final seconds = twoDigits(duration.inSeconds.remainder(60));
      
      _totalTimerSeconds++;

      timerString = '$minutes:$seconds';

      setState(() {
        final seconds = _totalTimerSeconds;
        duration = Duration(seconds: seconds);  
      });
    }
    else{
      timer?.cancel();
    }
  }
  
  void startTimer() {
    timer = Timer.periodic(const Duration(seconds: 1), (timer) => addTime());
  }
  
  @override
  Widget build(BuildContext context) {
    return Text(
      '($timerString)',
      style: TextStyle(
        fontSize:  widget.deviceHeight * 0.02,
      ),
    );
  }
}