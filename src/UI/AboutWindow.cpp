#include "AboutWindow.h"

#include <stdlib.h>
#include <stdio.h>
#include <imgui.h>

using namespace yade;

AboutWindow::AboutWindow(bool is_open)
	: EditorWindow(is_open, "About YADE", ImVec2(300,150))
{
}

void AboutWindow::drawContents() {
	ImGui::Text("Yet Another Doom Editor");
	ImGui::Text("Version 1.0 - CPlusPlus");
	ImGui::Separator();
	ImGui::Text("Created by Ashifolfi");
	ImGui::Separator();
	if (ImGui::Button("Close"))
	{
		this->show = false;
	}
}