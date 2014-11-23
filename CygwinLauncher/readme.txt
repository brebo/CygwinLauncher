================================================================================
  CygwinLauncher
================================================================================

【 ソフト名 】CygwinLauncher
【 製 作 者 】ぶれぼ
【 開発環境 】Visual C# 2012
【バージョン】1.0.0
【最終更新日】2014/11/24
【 ウ ェ ブ 】http://www.pantherweb.net/
【 E - mail 】brebo@pantherweb.net

---------- ----------
◇ 概要 ◇
このソフトは、PuTTy から SSH 経由で Cygwin を利用するための
起動を補助するソフトです。

PuTTy 以外でも動作する可能性はありますが、サポートは致しません。

◇ 動作条件 ◇
.NET framework 4.5 がインストールされていること。
※.NET framework 4.5 は次のリンクからダウンロードできます。
http://www.microsoft.com/ja-jp/download/details.aspx?id=30653

◇ ファイル構成 ◇
CygwinLauncher.exe: 本体です。
CygwinLauncherConfig.exe: 設定する際に使用する実行ファイルです。
readme.txt: このファイルです。

CommandTemplate.sh: Cygwin 上で実行するコマンドのテンプレートです。

以下は実行に必要なファイルです。
CommandLine.dll
CygwinLauncher.exe.config
CygwinLauncherBase.dll
CygwinLauncherConfig.exe.config
Microsoft.Practices.Prism.Composition.dll
Microsoft.Practices.Prism.Interactivity.dll
Microsoft.Practices.Prism.Mvvm.Desktop.dll
Microsoft.Practices.Prism.Mvvm.dll
Microsoft.Practices.Prism.PubSubEvents.dll
Microsoft.Practices.Prism.SharedInterfaces.dll
Microsoft.Practices.ServiceLocation.dll
System.Windows.Interactivity.dll

◇ 使用方法 ◇

□ 事前準備
・Cygwin + SSH の環境を構築する。
ssh-host-config で設定して、サービスを起動する。

・Putty 上で localhost に SSH で接続する設定を作成しておく。

・Cygwin のホームディレクトリの .bashrc に以下の記述を追加する。
if [ -f ${HOME}/.init_dir  ]; then
	cd "$(cygpath "$(cat ${HOME}/.init_dir)")"
	rm ${HOME}/.init_dir
fi

・CygwinLauncherConfig.exe を起動して以下の設定を行う。
PuTTy のパス：PuTTy のファイル (putty.exe) を指定してください。
PuTTy のパラメータ：-load "(PuTTyで設定した名前)" -ssh localhost
出力ファイルのパス：Cygwin のホームディレクトリの .init_dir ファイルを指定して下さい。

.init_dir ファイルは接続後に削除されます。
CygwinLauncher 起動時に .init_dir ファイルが存在した場合、
Cygwin 起動中とみなし、.init_dir が削除されるまで待機します。

その待機について、以下の設定を実施できます。
タイムアウト：ここで指定した時間が経過すると、何らかの理由で起動が失敗して
              ファイルが残ったものとみなし、ファイルを削除します。
チェック間隔：ここで指定した間隔で、.init_dir が削除されたかどうかをチェックします。
              短くすると .init_dir が削除されてすぐに反応できますが、
              チェックが頻繁になるので重くなる可能性があります。

□ CygwinLauncher 起動
(CygwinLauncherのあるディレクトリ)\CygwinLauncher.exe として起動すると、
そのときのカレントディレクトリで Cygwin に接続します。

(CygwinLauncherのあるディレクトリ)\CygwinLauncher.exe -d (ディレクトリ) として起動すると、
-d で指定したディレクトリをカレントディレクトリとして Cygwin に接続します。

(CygwinLauncherのあるディレクトリ)\CygwinLauncher.exe -c (シェルスクリプトのパス) として起動すると、
Cygwin に接続して、指定したシェルスクリプトを起動します。

□ 便利な使い方
(CygwinLauncherのあるディレクトリ)\CygwinLauncher.exe -c "%1" を .sh ファイルの
関連付けに設定すると、*.sh をダブルクリックすることで SSH 経由の Cygwin で
シェルスクリプトを実行できます。

(CygwinLauncherのあるディレクトリ)\CygwinLauncher.exe -d (ディレクトリ) をシェル拡張で
「Cygwin で開く」などとして起動できるようにすると、エクスプローラ上で右クリックすることで
指定したディレクトリで SSH 経由の Cygwin を起動できるようになります。

◇ 免責 ◇
本ソフトウェアに関係して発生した、いかなる障害や損害に対して、
作者は一切の責任を負いません。

◇ 更新履歴 ◇
Ver 1.0.0
・初回リリース。

◇ 使用ライブラリ ◇
・Command Line Parser Library：https://commandline.codeplex.com/
  ライセンス：MIT License
・Prism：http://msdn.microsoft.com/ja-jp/library/gg406140.aspx
  ライセンス：MSPL

