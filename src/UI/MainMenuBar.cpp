#include "MainMenuBar.h"

#include <stdio.h>
#include <stdlib.h>
#include <imgui.h>

using namespace yade;

MainMenuBar::MainMenuBar(AboutWindow abWin) {
	aboutWindow = abWin;
}

void MainMenuBar::drawMenuBar() {
	ImGui::BeginMainMenuBar();
	
	if (ImGui::BeginMenu("File")) 
	{
		ImGui::MenuItem("New");
		ImGui::Separator();
		ImGui::MenuItem("Open Archive");
		ImGui::MenuItem("Open Directory");
		ImGui::Separator();
		ImGui::MenuItem("Save");
		ImGui::MenuItem("Save As");
		ImGui::MenuItem("Save All");
		ImGui::Separator();
		ImGui::MenuItem("Close");
		ImGui::MenuItem("Close All");
		ImGui::Separator();
		//self::drawRecentFiles();
		ImGui::Text("TODO: Implement recent files");
		ImGui::Separator();
		if (ImGui::MenuItem("Exit"))
		{
			exit(EXIT_SUCCESS);
		}
		ImGui::EndMenu();
	}
	if (ImGui::BeginMenu("Help"))
	{
		if (ImGui::MenuItem("About"))
		{
			aboutWindow.show = true;
		}
		ImGui::EndMenu();
	}
	if (ImGui::BeginMenu("DEBUG"))
	{
		if (ImGui::MenuItem("Test CTexture Editor MRCE"))
		{
			//Game1.editor1 = new CTexture.Editor("MRCE");
		}
		if (ImGui::MenuItem("Test CTexture Editor SRB2"))
		{
			//Game1.editor1 = new CTexture.Editor("SRB2");
		}
		if (ImGui::MenuItem("Test Archive Editor MRCE.zip"))
		{
			//Resource.Archive archive = new Resource.Archive("MRCE.zip", "MRCE.zip");
			//Game1.openEditors.Add(new Archive.Editor(archive, Convert.ToString(Game1.openEditors.Count()+1)));
		}
		ImGui::EndMenu();
	}
	ImGui::EndMainMenuBar();
}