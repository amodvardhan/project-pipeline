'use client';

import { Button } from '@/components/ui/button';
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from '@/components/ui/dropdown-menu';
import { Download, FileSpreadsheet, FileText } from 'lucide-react';
import { Project } from '@/types';
import { exportProjectsToExcel, exportDashboardToPDF, exportProjectDetailToPDF } from '@/lib/exportUtils';

interface ExportButtonsProps {
  data?: Project[];
  type: 'projects' | 'dashboard' | 'single-project';
  stats?: any;
  singleProject?: Project;
}

export default function ExportButtons({ data = [], type, stats, singleProject }: ExportButtonsProps) {
  const handleExportExcel = () => {
    if (type === 'projects' && data.length > 0) {
      exportProjectsToExcel(data, 'projects_export');
    } else if (type === 'single-project' && singleProject) {
      // Export single project as Excel
      exportProjectsToExcel([singleProject], `project_${singleProject.name.replace(/[^a-zA-Z0-9]/g, '_')}`);
    }
  };

  const handleExportPDF = () => {
    if (type === 'dashboard' && stats) {
      exportDashboardToPDF(stats, data);
    } else if (type === 'projects' && data.length > 0) {
      // For projects list, create a summary PDF
      const projectStats = {
        totalProjects: data.length,
        wonProjects: data.filter(p => p.status === 'Won').length,
        pipelineProjects: data.filter(p => p.status === 'Pipeline').length,
        lostProjects: data.filter(p => p.status === 'Lost').length,
        totalValue: data.reduce((sum, p) => sum + (p.estimatedValue || 0), 0),
        wonValue: data.filter(p => p.status === 'Won').reduce((sum, p) => sum + (p.actualValue || p.estimatedValue || 0), 0),
        winRate: data.length > 0 ? (data.filter(p => p.status === 'Won').length / data.length) * 100 : 0,
        averageDealSize: data.filter(p => p.status === 'Won').length > 0 ? 
          data.filter(p => p.status === 'Won').reduce((sum, p) => sum + (p.actualValue || p.estimatedValue || 0), 0) / 
          data.filter(p => p.status === 'Won').length : 0
      };
      exportDashboardToPDF(projectStats, data);
    } else if (type === 'single-project' && singleProject) {
      exportProjectDetailToPDF(singleProject);
    }
  };

  // Don't render if no data available
  if (type === 'projects' && data.length === 0) {
    return null;
  }

  if (type === 'single-project' && !singleProject) {
    return null;
  }

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="outline" size="sm">
          <Download className="h-4 w-4 mr-2" />
          Export
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        {(type === 'projects' || type === 'single-project') && (
          <DropdownMenuItem onClick={handleExportExcel}>
            <FileSpreadsheet className="h-4 w-4 mr-2" />
            Export to Excel
          </DropdownMenuItem>
        )}
        <DropdownMenuItem onClick={handleExportPDF}>
          <FileText className="h-4 w-4 mr-2" />
          {type === 'single-project' ? 'Export Project PDF' : 'Export to PDF'}
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
